﻿using UnitsNet;
using UnitsNet.Units;

namespace ElektroSim
{
    public class Slovenia : ElectricSystem
    {
        public Slovenia()
        {
            HeDrava();
            HeSava();

            Freeze();
        }

        private void HeSava()
        {
            var dravaAfterFormin = CreateWaterBody("Hrvaška meja", new Volume(0, VolumeUnit.CubicHectometer), null);
            var dravaPtujskoJezero = CreateWaterBody("Ptujsko jezero", new Volume(4.5, VolumeUnit.CubicHectometer), dravaAfterFormin);
            var dravaAfterMariborskiOtok = CreateWaterBody("Za HE MariborskiOtok", new Volume(4.5, VolumeUnit.CubicHectometer), dravaPtujskoJezero);
            var dravaBeforeMariborskiOtok = CreateWaterBody("Pred HE MariborskiOtok", new Volume(2.1, VolumeUnit.CubicHectometer), dravaAfterMariborskiOtok);
            var dravaBeforeFala = CreateWaterBody("Pred HE Fala", new Volume(0.9, VolumeUnit.CubicHectometer), dravaBeforeMariborskiOtok);
            var predVrhovo = CreateWaterBody("Pred HE Ožbalt", new Volume(1.16, VolumeUnit.CubicHectometer), dravaBeforeFala);
            var predMedvode = CreateWaterBody("Pred HE Medvode", new Volume(1.12, VolumeUnit.CubicHectometer), predVrhovo);
            var predHEMavcice = CreateWaterBody("Pred HE Mavčiče", new Volume(1.68, VolumeUnit.CubicHectometer), predMedvode);
            var predHEMoste = CreateWaterBody("Sava dolinka pred HE Moste", new Volume(2.94, VolumeUnit.CubicHectometer), predHEMavcice);

            CreateHydroPowerPlant(
                "HE Moste",
                predHEMoste,
                predHEMavcice,
                VolumeFlow.FromCubicMetersPerSecond(26),
                Power.FromMegawatts(13));
            CreateHydroPowerPlant(
                "HE Mavčiče",
                predHEMavcice,
                predMedvode,
                VolumeFlow.FromCubicMetersPerSecond(260),
                Power.FromMegawatts(38));
            CreateHydroPowerPlant(
                "HE Medvode",
                predMedvode,
                predVrhovo,
                VolumeFlow.FromCubicMetersPerSecond(150),
                Power.FromMegawatts(25));
            CreateHydroPowerPlant(
                "HE Vrhovo",
                predVrhovo,
                dravaBeforeFala,
                VolumeFlow.FromCubicMetersPerSecond(550),
                Power.FromMegawatts(34));
            CreateHydroPowerPlant(
                "HE Fala",
                dravaBeforeFala,
                dravaBeforeMariborskiOtok,
                VolumeFlow.FromCubicMetersPerSecond(525),
                Power.FromMegawatts(58));
            CreateHydroPowerPlant(
                "HE Mariborski Otok",
                dravaBeforeMariborskiOtok,
                dravaAfterMariborskiOtok,
                VolumeFlow.FromCubicMetersPerSecond(550),
                Power.FromMegawatts(60));
            CreateHydroPowerPlant(
                "HE Zlatoličje",
                dravaAfterMariborskiOtok,
                dravaPtujskoJezero,
                VolumeFlow.FromCubicMetersPerSecond(530),
                Power.FromMegawatts(136),
                VolumeFlow.FromCubicMetersPerSecond(10));
            CreateHydroPowerPlant(
                "HE Formin",
                dravaPtujskoJezero,
                dravaAfterFormin,
                VolumeFlow.FromCubicMetersPerSecond(500),
                Power.FromMegawatts(116),
                VolumeFlow.FromCubicMetersPerSecond(10));
            CreateHydroPowerPlant(
                "MHE Markovci",
                dravaPtujskoJezero,
                dravaAfterFormin,
                VolumeFlow.FromCubicMetersPerSecond(29.75),
                Power.FromMegawatts(0.9),
                VolumeFlow.FromCubicMetersPerSecond(10));
            CreateHydroPowerPlant(
                "MHE Melje",
                dravaAfterMariborskiOtok,
                dravaPtujskoJezero,
                VolumeFlow.FromCubicMetersPerSecond(33),
                Power.FromMegawatts(2.26),
                VolumeFlow.FromCubicMetersPerSecond(10));
        }

        private void HeDrava()
        {
            var dravaAfterFormin = CreateWaterBody("Hrvaška meja", new Volume(0, VolumeUnit.CubicHectometer), null);
            var dravaPtujskoJezero = CreateWaterBody("Ptujsko jezero", new Volume(4.5, VolumeUnit.CubicHectometer), dravaAfterFormin);
            var dravaAfterMariborskiOtok = CreateWaterBody("Za HE MariborskiOtok", new Volume(4.5, VolumeUnit.CubicHectometer), dravaPtujskoJezero);
            var dravaBeforeMariborskiOtok = CreateWaterBody("Pred HE MariborskiOtok", new Volume(2.1, VolumeUnit.CubicHectometer), dravaAfterMariborskiOtok);
            var dravaBeforeFala = CreateWaterBody("Pred HE Fala", new Volume(0.9, VolumeUnit.CubicHectometer), dravaBeforeMariborskiOtok);
            var dravaBeforeOžbalt = CreateWaterBody("Pred HE Ožbalt", new Volume(1.4, VolumeUnit.CubicHectometer), dravaBeforeFala);
            var dravaBeforeVuhred = CreateWaterBody("Pred HE Vuhred", new Volume(2.2, VolumeUnit.CubicHectometer), dravaBeforeOžbalt);
            var dravaBeforeVuzenica = CreateWaterBody("Pred HE Vuzenica", new Volume(1.8, VolumeUnit.CubicHectometer), dravaBeforeVuhred);
            var dravaBeforeDravograd = CreateWaterBody("Avstrijska meja", new Volume(2, VolumeUnit.CubicHectometer), dravaBeforeVuzenica);

            CreateHydroPowerPlant(
                "HE Dravograd",
                dravaBeforeDravograd,
                dravaBeforeVuzenica,
                VolumeFlow.FromCubicMetersPerSecond(405),
                Power.FromMegawatts(26.2));
            CreateHydroPowerPlant(
                "HE Vuznica",
                dravaBeforeVuzenica,
                dravaBeforeVuhred,
                VolumeFlow.FromCubicMetersPerSecond(550),
                Power.FromMegawatts(55.6));
            CreateHydroPowerPlant(
                "HE Vuhred",
                dravaBeforeVuhred,
                dravaBeforeOžbalt,
                VolumeFlow.FromCubicMetersPerSecond(550),
                Power.FromMegawatts(72.3));
            CreateHydroPowerPlant(
                "HE Ožbalt",
                dravaBeforeOžbalt,
                dravaBeforeFala,
                VolumeFlow.FromCubicMetersPerSecond(550),
                Power.FromMegawatts(73.2));
            CreateHydroPowerPlant(
                "HE Fala",
                dravaBeforeFala,
                dravaBeforeMariborskiOtok,
                VolumeFlow.FromCubicMetersPerSecond(525),
                Power.FromMegawatts(58));
            CreateHydroPowerPlant(
                "HE Mariborski Otok",
                dravaBeforeMariborskiOtok,
                dravaAfterMariborskiOtok,
                VolumeFlow.FromCubicMetersPerSecond(550),
                Power.FromMegawatts(60));
            CreateHydroPowerPlant(
                "HE Zlatoličje",
                dravaAfterMariborskiOtok,
                dravaPtujskoJezero,
                VolumeFlow.FromCubicMetersPerSecond(530),
                Power.FromMegawatts(136),
                VolumeFlow.FromCubicMetersPerSecond(10));
            CreateHydroPowerPlant(
                "HE Formin",
                dravaPtujskoJezero,
                dravaAfterFormin,
                VolumeFlow.FromCubicMetersPerSecond(500),
                Power.FromMegawatts(116),
                VolumeFlow.FromCubicMetersPerSecond(10));
            CreateHydroPowerPlant(
                "MHE Markovci",
                dravaPtujskoJezero,
                dravaAfterFormin,
                VolumeFlow.FromCubicMetersPerSecond(29.75),
                Power.FromMegawatts(0.9),
                VolumeFlow.FromCubicMetersPerSecond(10));
            CreateHydroPowerPlant(
                "MHE Melje",
                dravaAfterMariborskiOtok,
                dravaPtujskoJezero,
                VolumeFlow.FromCubicMetersPerSecond(33),
                Power.FromMegawatts(2.26),
                VolumeFlow.FromCubicMetersPerSecond(10));
        }
    }
}

