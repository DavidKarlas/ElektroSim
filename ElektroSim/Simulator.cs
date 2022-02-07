using ElektroSim;
using ElektroSim.HistoricData;
using UnitsNet;

class Simulator
{
    public Simulator(Resolution resolution, ElectricSystem electricSystem, ENTSOE_Load load, ArsoWaterFlow arsoWaterFlow)
    {
        Resolution = resolution;
        ElectricSystem = electricSystem;
        Load = load;
        ArsoWaterFlow = arsoWaterFlow;

        Initialize();
    }

    private void Initialize()
    {
        foreach (var waterBody in ElectricSystem.WaterBodies)
        {
            //waterBody.CurrentPoolSize = waterBody.PoolSize / 2;
            waterBody.Overflows = new Volume[Resolution.NumberOfBrackets];
            waterBody.Levels = new Volume[Resolution.NumberOfBrackets];
        }

        foreach (var powerSource in ElectricSystem.HydroPowerPlants)
        {
            powerSource.Produced = new Energy[Resolution.NumberOfBrackets];
        }
    }

    public Resolution Resolution { get; }
    public ElectricSystem ElectricSystem { get; }
    public ENTSOE_Load Load { get; }
    public ArsoWaterFlow ArsoWaterFlow { get; }

    public void Run()
    {
        for (int i = 0; i < Resolution.NumberOfBrackets; i++)
        {
            AddInflowsToWaterBodies(i);
            ProduceElectricity(i);
            PrintStatus(i);
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
        foreach (var powerPlants in ElectricSystem.HydroPowerPlants)
        {
            sw.Write($"{powerPlants.Name} - power;");
        }

        foreach (var namedInflow in ArsoWaterFlow.NamedInflows)
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
            foreach (var powerPlant in ElectricSystem.HydroPowerPlants)
            {
                sw.Write($"{powerPlant.Produced[i].As(UnitsNet.Units.EnergyUnit.MegawattHour)};");
            }
            foreach (var namedInflow in ArsoWaterFlow.NamedInflows)
            {
                sw.Write($"{namedInflow.Value[i].As(UnitsNet.Units.VolumeUnit.CubicMeter)};");
            }

            sw.WriteLine();

        }
    }

    private void PrintFinalReport()
    {
        Console.WriteLine($"================================");
        Console.WriteLine($"================================");
        foreach (var powerPlant in ElectricSystem.HydroPowerPlants)
        {
            Console.WriteLine($"{powerPlant.Name}: {UnitMath.Sum(powerPlant.Produced, UnitsNet.Units.EnergyUnit.GigawattHour)}");
        }
        foreach (var waterBody in ElectricSystem.WaterBodies)
        {
            Console.WriteLine($"{waterBody.Name}: {waterBody.CurrentPoolSize}({waterBody.CurrentPoolSize / waterBody.MaxPoolSize})");
        }
    }

    private void PrintStatus(int i)
    {
        var time = Resolution.GetTime(i);
        if (time.TimeOfDay.TotalSeconds == 0)
        {
            Console.WriteLine($"Time: {Resolution.GetTime(i)}");
            foreach (var powerPlant in ElectricSystem.HydroPowerPlants)
            {
                Console.WriteLine($"{powerPlant.Name}: {powerPlant.Produced[i].As(UnitsNet.Units.EnergyUnit.MegawattHour):0.00}");
            }
            foreach (var waterBody in ElectricSystem.WaterBodies)
            {
                Console.WriteLine($"{waterBody.Name}: {waterBody.CurrentPoolSize.As(UnitsNet.Units.VolumeUnit.CubicHectometer):0.00 hm3}({waterBody.CurrentPoolSize / waterBody.MaxPoolSize:0.00%})");
            }
            Console.WriteLine($"================================");
        }
    }

    private void ProduceElectricity(int i)
    {
        foreach (var hydroPlant in ElectricSystem.HydroPowerPlants)
        {
            if (hydroPlant.MinFlow.CubicMetersPerSecond == 0)
                continue;
            var drainSize = UnitMath.Min(hydroPlant.WaterBodyBefore.CurrentPoolSize, hydroPlant.MinFlow.Divide(Resolution.Precision));
            hydroPlant.WaterBodyBefore.CurrentPoolSize -= drainSize;
            hydroPlant.Produced[i] += hydroPlant.CalcEnergy(drainSize);
            hydroPlant.WaterBodyAfter.CurrentPoolSize += drainSize;
        }
        foreach (var hydroPlant in ElectricSystem.HydroPowerPlants)
        {
            var drainSize = UnitMath.Min(hydroPlant.WaterBodyBefore.CurrentPoolSize, (hydroPlant.MaxFlow - hydroPlant.MinFlow).Divide(Resolution.Precision));
            hydroPlant.WaterBodyBefore.CurrentPoolSize -= drainSize;
            hydroPlant.Produced[i] += hydroPlant.CalcEnergy(drainSize);
            hydroPlant.WaterBodyAfter.CurrentPoolSize += drainSize;
        }

        DoOverflowing(i);
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
        foreach (var inflow in ArsoWaterFlow.NamedInflows)
        {
            var waterBody = ElectricSystem.NamedWaterBodies[inflow.Key];
            waterBody.CurrentPoolSize += inflow.Value[i];
        }
    }
}