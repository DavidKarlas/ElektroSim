using System;
using ElektroSim.HistoricData;
using UnitsNet;

namespace ElektroSim.Elements
{
    public class SolarPowerSource : PowerSource
    {
        public SolarPowerSource(string name, Power[] pastMaxPower, Power[] pastPerf) : base(name)
        {
            PastMaxPower = pastMaxPower;
            PastPerf = pastPerf;
        }

        public Power[] PastMaxPower { get; }
        public Power[] PastPerf { get; }
    }
}
