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
    public List<HydroPowerPlant> HydroPowerPlants { get; } = new();
    public List<PumpedHydroPowerPlant> PumpedHydroPowerPlants { get; } = new();
    public List<NuclearPowerPlant> NuclearPowerPlants { get; } = new();
    public List<PowerSource> PowerSources { get; } = new();

    public WaterBody CreateWaterBody(string name, Volume poolSize, WaterBody? overlowTo)
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
            OverflowTo = overlowTo
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
        HydroPowerPlants.Add(newPowerPlant);
        return newPowerPlant;
    }

    public HydroPowerPlant CreateFossilPowerPlant(string name, Power minPower, Power maxPower, Mass co2PerMegawattHour, Duration minWorktime)
    {
        if (IsFrozen)
            throw new Exception();
        var newPowerPlant = new FossilPowerPlant(name, minPower, maxPower, co2PerMegawattHour, minWorktime);
        PowerSources.Add(newPowerPlant);
        return newPowerPlant;
    }

    public HydroPowerPlant CreateNuclearPowerPlant(string name, Power minPower, Power maxPower)
    {
        if (IsFrozen)
            throw new Exception();
        var newPowerPlant = new NuclearPowerPlant(name, minPower, maxPower);
        NuclearPowerPlants.Add(newPowerPlant);
        return newPowerPlant;
    }

    public HydroPowerPlant CreatePumpedHydroPowerPlant(
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
        PumpedHydroPowerPlants.Add(newPowerPlant);
        return newPowerPlant;
    }
}