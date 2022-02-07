using System;
using UnitsNet;

namespace ElektroSim.Elements
{
    public class PowerSource
    {
        public PowerSource(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public Energy[] Produced { get; set; } = Array.Empty<Energy>();
    }
}

