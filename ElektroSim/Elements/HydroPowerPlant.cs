using System;
using UnitsNet;

namespace ElektroSim.Elements
{
    public class HydroPowerPlant : PowerSource
    {
        public WaterBody WaterBodyBefore { get; init; }
        public WaterBody WaterBodyAfter { get; init; }
        public VolumeFlow MaxFlow { get; init; }
        public VolumeFlow MinFlow { get; init; }
        public Power MaxPower { get; init; }

        public HydroPowerPlant(string name, WaterBody waterBodyBefore, WaterBody waterBodyAfter, VolumeFlow maxFlow, Power maxPower, VolumeFlow minFlow = default)
            : base(name)
        {
            WaterBodyBefore = waterBodyBefore;
            WaterBodyAfter = waterBodyAfter;
            MaxFlow = maxFlow;
            MaxPower = maxPower;
            MinFlow = minFlow;
        }

        public Energy CalcEnergy(Volume drainSize)
        {
            return Energy.FromWattHours(MaxPower.Watts * (drainSize.CubicMeters / MaxFlow.CubicMetersPerHour));
        }
    }
}

