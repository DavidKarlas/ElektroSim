using ElektroSim;
using ElektroSim.Elements;
using ElektroSim.HistoricData;
using UnitsNet;
using UnitsNet.Units;

class Simulator
{
    public Simulator(Resolution resolution, ElectricSystem electricSystem, ENTSOE_Data load, ArsoWaterFlow arsoWaterFlow)
    {
        Resolution = resolution;
        ElectricSystem = electricSystem;
        Load = load;
        ArsoWaterFlow = arsoWaterFlow;

        foreach (var waterBody in ElectricSystem.WaterBodies)
        {
            //waterBody.CurrentPoolSize = waterBody.PoolSize / 2;
            waterBody.Overflows = new Volume[Resolution.NumberOfBrackets];
            waterBody.Levels = new Volume[Resolution.NumberOfBrackets];
        }

        foreach (var powerSource in ElectricSystem.PowerSources)
        {
            powerSource.Produced = new Power[Resolution.NumberOfBrackets];
        }
        Import = new Power[Resolution.NumberOfBrackets];
        Export = new Power[Resolution.NumberOfBrackets];
    }

    public Resolution Resolution { get; }
    public ElectricSystem ElectricSystem { get; }
    public ENTSOE_Data Load { get; }
    public ArsoWaterFlow ArsoWaterFlow { get; }
    public Power[] Import { get; }
    public Power[] Export { get; }

    public void Run()
    {
        for (int i = 0; i < Resolution.NumberOfBrackets; i++)
        {
            AddInflowsToWaterBodies(i);
            ProduceElectricity(i);
            //PrintStatus(i);
        }
        OutputCsv();
        PrintFinalReport();
    }

    private void OutputCsv()
    {
        using var sw = new StreamWriter("output.csv");
        sw.Write("time;");
        foreach (var waterBody in ElectricSystem.WaterBodies)
        {
            sw.Write($"{waterBody.Name} - overflow;");
            sw.Write($"{waterBody.Name} - level;");
        }
        foreach (var powerPlants in ElectricSystem.PowerSources)
        {
            sw.Write($"{powerPlants.Name} - power;");
        }

        foreach (var namedInflow in ArsoWaterFlow.Inflows)
        {
            sw.Write($"{namedInflow.Key} - inflow;");
        }

        sw.WriteLine();

        for (int i = 0; i < Resolution.NumberOfBrackets; i++)
        {
            sw.Write($"{Resolution.GetTime(i)};");
            foreach (var waterBody in ElectricSystem.WaterBodies)
            {
                sw.Write($"{waterBody.Overflows[i].As(UnitsNet.Units.VolumeUnit.CubicMeter)};");
                sw.Write($"{waterBody.Levels[i].As(UnitsNet.Units.VolumeUnit.CubicMeter)};");
            }
            foreach (var powerPlant in ElectricSystem.PowerSources)
            {
                sw.Write($"{powerPlant.Produced[i].As(UnitsNet.Units.PowerUnit.Megawatt)};");
            }
            foreach (var namedInflow in ArsoWaterFlow.Inflows)
            {
                sw.Write($"{namedInflow.Value[i].As(UnitsNet.Units.VolumeUnit.CubicMeter)};");
            }

            sw.WriteLine();

        }
    }

    private void PrintFinalReport()
    {
        foreach (var waterBody in ElectricSystem.WaterBodies.OrderBy(wb => UnitMath.Sum(wb.Overflows, VolumeUnit.CubicHectometer)))
        {
            Console.WriteLine($"{waterBody.Name}: {UnitMath.Sum(waterBody.Overflows, VolumeUnit.CubicHectometer)}");
        }
        foreach (var powerPlant in ElectricSystem.PowerSources.OrderBy(p => p.GetType().FullName).ThenBy(p => p.Name))
        {
            Console.WriteLine($"{powerPlant.Name}: {Energy.FromGigawattHours((UnitMath.Sum(powerPlant.Produced, UnitsNet.Units.PowerUnit.Gigawatt) * Duration.FromHours(1)).GigawattHours)}");
        }
        Console.WriteLine($"Import: {Energy.FromGigawattHours((UnitMath.Sum(Import, UnitsNet.Units.PowerUnit.Gigawatt) * Duration.FromHours(1)).GigawattHours)}");
        Console.WriteLine($"Export: {Energy.FromGigawattHours((UnitMath.Sum(Export, UnitsNet.Units.PowerUnit.Gigawatt) * Duration.FromHours(1)).GigawattHours)}");
    }

    private void ProduceElectricity(int i)
    {
        var load = Load.Load[i];

        foreach (var nuclear in ElectricSystem.PowerSources.OfType<NuclearPowerPlant>())
        {
            nuclear.Produced[i] = nuclear.MaxPower;
            load -= nuclear.MaxPower;
        }

        foreach (var hydroPlant in ElectricSystem.PowerSources.OfType<HydroPowerPlant>())
        {
            if (hydroPlant is PumpedHydroPowerPlant)
                continue;
            if (hydroPlant.MinFlow.CubicMetersPerSecond == 0)
                continue;
            var drainSize = UnitMath.Min(hydroPlant.WaterBodyBefore.CurrentPoolSize, hydroPlant.MinFlow.Divide(Resolution.Precision));
            hydroPlant.WaterBodyBefore.CurrentPoolSize -= drainSize;
            var power = hydroPlant.CalcPower(drainSize);
            hydroPlant.Produced[i] += power;
            hydroPlant.WaterBodyAfter.CurrentPoolSize += drainSize;
            load -= power;
        }

        foreach (var solar in ElectricSystem.PowerSources.OfType<SolarPowerSource>())
        {
            var power = Power.FromMegawatts((solar.PastPerf[i] / solar.PastMaxPower[i]) * solar.SimulatedMaxPower[i].Megawatts);
            solar.Produced[i] = power;
            load -= power;
        }

        foreach (var hydroPlant in ElectricSystem.PowerSources.OfType<HydroPowerPlant>().OrderBy(hpp => hpp.WaterBodyAfter.PoolPercetage))
        {
            if (hydroPlant is PumpedHydroPowerPlant)
                continue;

            var drainSize = UnitMath.Min(hydroPlant.WaterBodyBefore.CurrentPoolSize, (hydroPlant.MaxFlow - hydroPlant.MinFlow).Divide(Resolution.Precision));
            var power = hydroPlant.CalcPower(drainSize);
            if (load - power < Power.Zero)
                break;
            hydroPlant.WaterBodyBefore.CurrentPoolSize -= drainSize;
            hydroPlant.Produced[i] += power;
            hydroPlant.WaterBodyAfter.CurrentPoolSize += drainSize;
            load -= power;
        }

        DoOverflowing(i);

        if (load > Power.Zero)
        {
            Import[i] = load;
        }
        else
        {
            Export[i] = -load;
        }
    }

    private bool DoOverflowing(int i)
    {
        bool overflowing = false;
        foreach (var waterBody in ElectricSystem.WaterBodies)
        {
            if (waterBody.CurrentPoolSize > waterBody.MaxPoolSize && waterBody.OverflowTo != null)
            {
                overflowing = true;
                var volumeToOverflow = waterBody.CurrentPoolSize - waterBody.MaxPoolSize;
                waterBody.OverflowTo.CurrentPoolSize += volumeToOverflow;
                waterBody.CurrentPoolSize = waterBody.MaxPoolSize;
                waterBody.Overflows[i] += volumeToOverflow;
            }
            waterBody.Levels[i] = waterBody.CurrentPoolSize;
        }
        return overflowing;
    }

    private void AddInflowsToWaterBodies(int i)
    {
        foreach (var waterBody in ElectricSystem.WaterBodies)
        {
            foreach (var inflowId in waterBody.InFlows)
            {
                waterBody.CurrentPoolSize += ArsoWaterFlow.Inflows[inflowId][i];
            }
        }
    }
}