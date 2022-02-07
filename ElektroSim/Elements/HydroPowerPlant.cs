using System;
using UnitsNet;

namespace ElektroSim.Elements
{
    public class HydroPowerPlant : PowerSource
    {
        public WaterBody WaterBodyBefore { get; init; }
        public WaterBody WaterBodyAfter { get; init; }
        public VolumeFlow MaxFlow { get; init; }
        public Power MaxPower { get; init; }
    }
}

