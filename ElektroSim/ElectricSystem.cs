using ElektroSim.Elements;
using UnitsNet;
using UnitsNet.Units;

public abstract class ElectricSystem
{
    public bool IsFrozen { get; private set; }
    public void Freeze()
    {
        WaterBodies.Reverse();
        IsFrozen = true;
    }

    public List<WaterBody> WaterBodies { get; } = new();
    public Dictionary<string, WaterBody> NamedWaterBodies { get; } = new();
    public List<PowerSource> PowerSources { get; } = new();

    public WaterBody CreateWaterBody(string name, Volume poolSize, WaterBody? overlowTo, params int[] inFlows)
    {
        if (IsFrozen)
            throw new Exception();
        if (overlowTo == null && poolSize != Volume.Zero)
        {
            throw new Exception("If water body has size it also needs to have overflow");
        }

        var newWaterBody = new WaterBody()
        {
            Name = name,
            MaxPoolSize = poolSize,
            OverflowTo = overlowTo,
            InFlows = inFlows
        };
        WaterBodies.Add(newWaterBody);
        if (name != null)
        {
            NamedWaterBodies.Add(name, newWaterBody);
        }
        return newWaterBody;
    }

    public HydroPowerPlant CreateHydroPowerPlant(string name, WaterBody waterBodyBefore, WaterBody waterBodyAfter, VolumeFlow maxFlow, Power maxPower, VolumeFlow minFlow = default)
    {
        if (IsFrozen)
            throw new Exception();
        var newPowerPlant = new HydroPowerPlant(name, waterBodyBefore,
            waterBodyAfter,
            maxFlow,
            maxPower,
            minFlow);
        PowerSources.Add(newPowerPlant);
        return newPowerPlant;
    }

    public FossilPowerPlant CreateFossilPowerPlant(string name, Power minPower, Power maxPower, Mass co2PerMegawattHour, Duration minWorktime)
    {
        if (IsFrozen)
            throw new Exception();
        var newPowerPlant = new FossilPowerPlant(name, minPower, maxPower, co2PerMegawattHour, minWorktime);
        PowerSources.Add(newPowerPlant);
        return newPowerPlant;
    }

    public NuclearPowerPlant CreateNuclearPowerPlant(string name, Power minPower, Power maxPower)
    {
        if (IsFrozen)
            throw new Exception();
        var newPowerPlant = new NuclearPowerPlant(name, minPower, maxPower);
        PowerSources.Add(newPowerPlant);
        return newPowerPlant;
    }

    public SolarPowerSource CreateSolarPowerPlant(string name, Power[] pastMaxPower, Power[] pastPerf)
    {
        if (IsFrozen)
            throw new Exception();
        var newPowerPlant = new SolarPowerSource(name, pastMaxPower, pastPerf, pastMaxPower);
        PowerSources.Add(newPowerPlant);
        return newPowerPlant;
    }

    public PumpedHydroPowerPlant CreatePumpedHydroPowerPlant(
        string name,
        WaterBody waterBodyLower,
        WaterBody waterBodyUpper,
        Power maxPower,
        VolumeFlow maxFlow,
        VolumeFlow minFlow,
        Power maxPumpingPower,
        VolumeFlow maxPumpingFlow,
        VolumeFlow minPumpingFlow)
    {
        if (IsFrozen)
            throw new Exception();
        var newPowerPlant = new PumpedHydroPowerPlant(
            name,
            waterBodyLower,
            waterBodyUpper,
            maxPower,
            maxFlow,
            minFlow,
            maxPumpingPower,
            maxPumpingFlow,
            minPumpingFlow);
        PowerSources.Add(newPowerPlant);
        return newPowerPlant;
    }
}