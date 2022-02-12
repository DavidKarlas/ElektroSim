using System;
using ElektroSim.Elements;
using UnitsNet;

namespace ElektroSim
{
    public class CalcMinPower
    {
        ElectricSystem electricSystem;
        public CalcMinPower(ElectricSystem electricSystem)
        {
            this.electricSystem = electricSystem;
            var list = new List<(HydroPowerPlant powerPlant, Volume flow)>();
            list.Add((GetPowerPLant("Melje"), Volume.FromCubicMeters(13.3)));
            list.Add((GetPowerPLant("Formin"), Volume.FromCubicMeters(10)));
            list.Add((GetPowerPLant("Zlatoli"), Volume.FromCubicMeters(13.1)));
            list.Add((GetPowerPLant("Markovci"), Volume.FromCubicMeters(10)));
            list.Add((GetPowerPLant("Medvode"), Volume.FromCubicMeters(19)));
            list.Add((GetPowerPLant("Vrhovo"), Volume.FromCubicMeters(76)));
            list.Add((GetPowerPLant("Blanca"), Volume.FromCubicMeters(70)));
            list.Add((GetPowerPLant("Boštjan"), Volume.FromCubicMeters(65)));
            list.Add((GetPowerPLant("Krško"), Volume.FromCubicMeters(63)));
            list.Add((GetPowerPLant("Brežice"), Volume.FromCubicMeters(60)));

            var power = Energy.FromMegawattHours(0);
            foreach (var item in list)
            {
                var newPower = item.powerPlant.CalcEnergy(item.flow * 3600);
                power += newPower;
                Console.WriteLine(item.powerPlant.Name + " " + newPower.As(UnitsNet.Units.EnergyUnit.MegawattHour));
            }
            Console.WriteLine("Total:" + power);
        }

        private HydroPowerPlant GetPowerPLant(string name)
        {
            return this.electricSystem.HydroPowerPlants.Single(p => p.Name.Contains(name));
        }
    }
}

