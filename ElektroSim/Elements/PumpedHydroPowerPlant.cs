using System;
using UnitsNet;

namespace ElektroSim.Elements
{
    public class PumpedHydroPowerPlant : HydroPowerPlant
    {
        public PumpedHydroPowerPlant(string name,
            WaterBody waterBodyLower,
            WaterBody waterBodyUpper,
            Power maxPower,
            VolumeFlow maxFlow,
            VolumeFlow minFlow,
            Power maxPumpingPower,
            VolumeFlow maxPumpingFlow,
            VolumeFlow minPumpingFlow)
            : base(name, waterBodyUpper, waterBodyLower, maxFlow, maxPower, minFlow)
        {
            MaxPumpingPower = maxPumpingPower;
            MaxPumpingFlow = maxPumpingFlow;
            MinPumpingFlow = minPumpingFlow;
        }

        public WaterBody WaterBodyUpper => WaterBodyBefore;
        public WaterBody WaterBodyLower => WaterBodyAfter;
        public VolumeFlow MaxPumpingFlow { get; }
        public VolumeFlow MinPumpingFlow { get; }
        public Power MaxPumpingPower { get; }
    }
}

