using System;
using UnitsNet;

namespace ElektroSim.Elements
{
    public class FossilPowerPlant : PowerSource
    {
        public FossilPowerPlant(string name, Power minPower, Power maxPower, Mass co2PerMegawattHour, Duration minWorktime) : base(name)
        {
        }
    }
}

