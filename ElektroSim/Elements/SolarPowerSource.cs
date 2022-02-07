using System;
using UnitsNet;

namespace ElektroSim.Elements
{
    public class SolarPowerSource : PowerSource
    {
        public SolarPowerSource(string name) : base(name)
        {
        }

        public Power MaxPower { get; init; }
    }
}

