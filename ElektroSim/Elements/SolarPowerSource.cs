using System;
using ElektroSim.HistoricData;
using UnitsNet;

namespace ElektroSim.Elements
{
    public class SolarPowerSource : PowerSource
    {
        public SolarPowerSource(string name, Power[] pastMaxPower, Power[] pastPerf, Power[] simulatedMaxPower) : base(name)
        {
            PastMaxPower = pastMaxPower;
            PastPerf = pastPerf;
            SimulatedMaxPower = simulatedMaxPower;
        }

        public Power[] PastMaxPower { get; }
        public Power[] SimulatedMaxPower { get; }
        public Power[] PastPerf { get; }
    }
}
