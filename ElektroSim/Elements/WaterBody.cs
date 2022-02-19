using System;
using UnitsNet;

namespace ElektroSim.Elements
{
    public class WaterBody
    {
        public string? Name;
        public Volume MaxPoolSize;
        public Volume CurrentPoolSize;
        public WaterBody? OverflowTo;
        public Volume[] Overflows;
        public Volume[] Levels;

        public int[] InFlows { get; init; }

        public double PoolPercetage
        {
            get
            {
                if (MaxPoolSize == Volume.Zero)
                    return 1;
                return CurrentPoolSize / MaxPoolSize;
            }
        }
    }
}

