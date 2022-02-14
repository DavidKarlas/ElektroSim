using System;
using UnitsNet;

namespace ElektroSim.Elements
{
    public class NuclearPowerPlant : PowerSource
    {
        public NuclearPowerPlant(string name, Power minPower, Power maxPower) : base(name)
        {
            MinPower = minPower;
            MaxPower = maxPower;
        }

        public Power MinPower { get; }
        public Power MaxPower { get; }
    }
}

