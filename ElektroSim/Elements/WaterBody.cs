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
    }
}

