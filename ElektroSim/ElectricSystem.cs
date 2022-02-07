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
}