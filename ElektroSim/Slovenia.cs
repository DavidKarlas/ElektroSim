using System;
using ElektroSim.Elements;
using UnitsNet;
using UnitsNet.Units;

namespace ElektroSim
{
    public class Slovenia
    {
        public Slovenia()
        {
            

            var dravaBeforeDravograd = new WaterBody()
            {
                PoolSize = new Volume(2, VolumeUnit.CubicHectometer)
            };
            var dravaBeforeVuzenica = new WaterBody()
            {
                PoolSize = new Volume(1.8, VolumeUnit.CubicHectometer)
            };
            var heDravograd = new HydroPowerPlant()
            {
                WaterBodyBefore = dravaBeforeDravograd,
                WaterBodyAfter = dravaBeforeVuzenica,
                MaxFlow = VolumeFlow.FromCubicMetersPerSecond(405),
                MaxPower = Power.FromMegawatts(26.2)
            };
            var dravaBeforeVuhred = new WaterBody()
            {
                PoolSize = new Volume(2.2, VolumeUnit.CubicHectometer)
            };
            var heVuzenica = new HydroPowerPlant()
            {
                WaterBodyBefore = dravaBeforeVuzenica,
                WaterBodyAfter = dravaBeforeVuhred,
                MaxFlow = VolumeFlow.FromCubicMetersPerSecond(550),
                MaxPower = Power.FromMegawatts(55.6)
            };
            var dravaBeforeOžbalt = new WaterBody()
            {
                PoolSize = new Volume(1.4, VolumeUnit.CubicHectometer)
            };
            var heVuhred = new HydroPowerPlant()
            {
                WaterBodyBefore = dravaBeforeVuhred,
                WaterBodyAfter = dravaBeforeOžbalt,
                MaxFlow = VolumeFlow.FromCubicMetersPerSecond(550),
                MaxPower = Power.FromMegawatts(72.3)
            };
            var dravaBeforeFala = new WaterBody()
            {
                PoolSize = new Volume(0.9, VolumeUnit.CubicHectometer)
            };
            var heOžbalt = new HydroPowerPlant()
            {
                WaterBodyBefore = dravaBeforeOžbalt,
                WaterBodyAfter = dravaBeforeFala,
                MaxFlow = VolumeFlow.FromCubicMetersPerSecond(550),
                MaxPower = Power.FromMegawatts(73.2)
            };
            var dravaBeforeMariborskiOtok = new WaterBody()
            {
                PoolSize = new Volume(2.1, VolumeUnit.CubicHectometer)
            };
            var heFala = new HydroPowerPlant()
            {
                WaterBodyBefore = dravaBeforeFala,
                WaterBodyAfter = dravaBeforeMariborskiOtok,
                MaxFlow = VolumeFlow.FromCubicMetersPerSecond(525),
                MaxPower = Power.FromMegawatts(58)
            };
            var dravaBeforeZlatoličje = new WaterBody()
            {
                PoolSize = new Volume(4.5, VolumeUnit.CubicHectometer)
            };
            var heMariborskiOtok = new HydroPowerPlant()
            {
                WaterBodyBefore = dravaBeforeMariborskiOtok,
                WaterBodyAfter = dravaBeforeZlatoličje,
                MaxFlow = VolumeFlow.FromCubicMetersPerSecond(550),
                MaxPower = Power.FromMegawatts(60)
            };
            var dravaPtujskoJezero = new WaterBody()
            {
                PoolSize = new Volume(0, VolumeUnit.CubicHectometer)
            };
            var heZlatoličje = new HydroPowerPlant()
            {
                WaterBodyBefore = dravaBeforeZlatoličje,
                WaterBodyAfter = dravaPtujskoJezero,
                MaxFlow = VolumeFlow.FromCubicMetersPerSecond(530),
                MaxPower = Power.FromMegawatts(136)
            };
            var dravaAfterFormin = new WaterBody()
            {
                PoolSize = new Volume(double.PositiveInfinity, VolumeUnit.CubicHectometer)
            };
            var heFormin = new HydroPowerPlant()
            {
                WaterBodyBefore = dravaPtujskoJezero,
                WaterBodyAfter = dravaAfterFormin,
                MaxFlow = VolumeFlow.FromCubicMetersPerSecond(500),
                MaxPower = Power.FromMegawatts(116)
            };

            var mheMarkovci = new HydroPowerPlant()
            {
                WaterBodyBefore = dravaPtujskoJezero,
                WaterBodyAfter = dravaAfterFormin,
                MaxFlow = VolumeFlow.FromCubicMetersPerSecond(29.75),
                MaxPower = Power.FromMegawatts(0.9)
            };
            var mheMelje = new HydroPowerPlant()
            {
                WaterBodyBefore = dravaBeforeZlatoličje,
                WaterBodyAfter = dravaPtujskoJezero,
                MaxFlow = VolumeFlow.FromCubicMetersPerSecond(33),
                MaxPower = Power.FromMegawatts(2.26)
            };
        }
    }
}

