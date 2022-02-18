using System;
using ElektroSim.HistoricData;
using UnitsNet;

namespace ElektroSim.Elements
{
    public class SolarPowerSource : PowerSource
    {
        public Power MaxPower { get; init; }

        public SolarPowerSource(string name, Power maxPower) : base(name)
        {
            MaxPower = maxPower;
        }
    }
}
