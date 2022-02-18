using UnitsNet;
using UnitsNet.Units;

namespace ElektroSim
{
    public class Slovenia : ElectricSystem
    {
        // Based on number from Avče data
        private const double MinFlowFactorForPumpedHydro = 4 / 600.0;

        public Slovenia()
        {
            HeDrava();
            HeSava();
            HeSoca();

            CreateNuclearPowerPlant("JE Krško", Power.FromMegawatts(696 / 2), Power.FromMegawatts(696 / 2));

            CreateFossilPowerPlant("TE Šoštanj - Blok 6", Power.FromMegawatts(542.3 * 0.42), Power.FromMegawatts(542.3), Mass.FromKilograms(870), Duration.FromDays(7));
            CreateFossilPowerPlant("TE Šoštanj - Blok 5", Power.FromMegawatts(305 * 0.42), Power.FromMegawatts(305), Mass.FromKilograms(870), Duration.FromDays(7));
            CreateFossilPowerPlant("TE Šoštanj - Plin 51", Power.FromMegawatts(42), Power.FromMegawatts(42), Mass.FromKilograms(554), Duration.FromHours(2));
            CreateFossilPowerPlant("TE Šoštanj - Plin 52", Power.FromMegawatts(42), Power.FromMegawatts(42), Mass.FromKilograms(554), Duration.FromHours(2));

            Freeze();
        }

        private void HeSoca()
        {
            var poSolkanu = CreateWaterBody("Soča - Italijanska meja", new Volume(0, VolumeUnit.CubicHectometer), null);
            var predSolkan = CreateWaterBody("Pred HE Solkan", new Volume(3.4, VolumeUnit.CubicHectometer), poSolkanu);
            var predPlave = CreateWaterBody("Pred HE Plave", new Volume(1.18, VolumeUnit.CubicHectometer), predSolkan);
            var avceJezero = CreateWaterBody("Avče jezero", new Volume(2.2, VolumeUnit.CubicHectometer), predPlave);
            var predDoblar = CreateWaterBody("Pred HE Doblar", new Volume(1.3, VolumeUnit.CubicHectometer), predPlave);

            CreateHydroPowerPlant(
                "HE Doblar 1",
                predDoblar,
                predPlave,
                VolumeFlow.FromCubicMetersPerSecond(75),
                Power.FromMegawatts(30));
            CreateHydroPowerPlant(
                "HE Doblar 2",
                predDoblar,
                predPlave,
                VolumeFlow.FromCubicMetersPerSecond(105),
                Power.FromMegawatts(40));
            CreatePumpedHydroPowerPlant(
                "ČHE Avče",
                predPlave,
                avceJezero,
                Power.FromMegawatts(185),
                VolumeFlow.FromCubicMetersPerSecond(40),
                VolumeFlow.FromCubicMetersPerSecond(40 * MinFlowFactorForPumpedHydro),
                Power.FromMegawatts(180),
                VolumeFlow.FromCubicMetersPerSecond(34),
                VolumeFlow.FromCubicMetersPerSecond(34 * MinFlowFactorForPumpedHydro));
            CreateHydroPowerPlant(
                "HE Plave 1",
                predPlave,
                predSolkan,
                VolumeFlow.FromCubicMetersPerSecond(75),
                Power.FromMegawatts(15));
            CreateHydroPowerPlant(
                "HE Plave 2",
                predPlave,
                predSolkan,
                VolumeFlow.FromCubicMetersPerSecond(105),
                Power.FromMegawatts(20));
            CreateHydroPowerPlant(
                "MHE Ajba",
                predPlave,
                predSolkan,
                VolumeFlow.FromCubicMetersPerSecond(2.5),
                Power.FromMegawatts(0.25));
            CreateHydroPowerPlant(
                "HE Solkan",
                predSolkan,
                poSolkanu,
                VolumeFlow.FromCubicMetersPerSecond(180),
                Power.FromMegawatts(32));
        }


        private void HeSava()
        {
            var poBrežice = CreateWaterBody("Sava - Hrvaška meja", new Volume(0, VolumeUnit.CubicHectometer), null);
            var predBrežice = CreateWaterBody("Pred HE Brežice", new Volume(3.4, VolumeUnit.CubicHectometer), poBrežice);
            var predKrško = CreateWaterBody("Pred HE Krško", new Volume(1.18, VolumeUnit.CubicHectometer), predBrežice);
            var predBlanca = CreateWaterBody("Pred HE Blanca", new Volume(1.3, VolumeUnit.CubicHectometer), predKrško);
            var predBoštjanom = CreateWaterBody("Pred HE Boštjan", new Volume(1.17, VolumeUnit.CubicHectometer), predBlanca);
            var predVrhovo = CreateWaterBody("Pred HE Vrhovo", new Volume(1.16, VolumeUnit.CubicHectometer), predBoštjanom);
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
                predBoštjanom,
                VolumeFlow.FromCubicMetersPerSecond(501),
                Power.FromMegawatts(34));
            CreateHydroPowerPlant(
                "HE Boštjan",
                predBoštjanom,
                predBlanca,
                VolumeFlow.FromCubicMetersPerSecond(500),
                Power.FromMegawatts(32.5));
            CreateHydroPowerPlant(
                "HE Blanca",
                predBlanca,
                predKrško,
                VolumeFlow.FromCubicMetersPerSecond(500),
                Power.FromMegawatts(39.12));
            CreateHydroPowerPlant(
                "HE Krško",
                predKrško,
                predBrežice,
                VolumeFlow.FromCubicMetersPerSecond(500),
                Power.FromMegawatts(39.12));
            CreateHydroPowerPlant(
                "HE Brežice",
                predBrežice,
                poBrežice,
                VolumeFlow.FromCubicMetersPerSecond(500),
                Power.FromMegawatts(47.4));
        }

        private void HeDrava()
        {
            var dravaAfterFormin = CreateWaterBody("Drava - Hrvaška meja", new Volume(0, VolumeUnit.CubicHectometer), null);
            var dravaPtujskoJezero = CreateWaterBody("Ptujsko jezero", new Volume(4.5, VolumeUnit.CubicHectometer), dravaAfterFormin);
            var dravaAfterMariborskiOtok = CreateWaterBody("Za HE MariborskiOtok", new Volume(4.5, VolumeUnit.CubicHectometer), dravaPtujskoJezero);
            var dravaBeforeMariborskiOtok = CreateWaterBody("Pred HE MariborskiOtok", new Volume(2.1, VolumeUnit.CubicHectometer), dravaAfterMariborskiOtok);
            var dravaBeforeFala = CreateWaterBody("Pred HE Fala", new Volume(0.9, VolumeUnit.CubicHectometer), dravaBeforeMariborskiOtok);
            var kozjakJezero = CreateWaterBody("ČHE Kozjak jezero", new Volume(2.892, VolumeUnit.CubicHectometer), dravaBeforeFala);
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
                kozjakJezero,
                VolumeFlow.FromCubicMetersPerSecond(550),
                Power.FromMegawatts(73.2));
            CreatePumpedHydroPowerPlant(
                "ČHE Avče",
                dravaBeforeFala,
                kozjakJezero,
                Power.FromMegawatts(2 * 202.5),
                VolumeFlow.FromCubicMetersPerSecond(71.4),
                VolumeFlow.FromCubicMetersPerSecond(71.4 * MinFlowFactorForPumpedHydro),
                Power.FromMegawatts(2 * 212.8),
                VolumeFlow.FromCubicMetersPerSecond(56.6),
                VolumeFlow.FromCubicMetersPerSecond(56.6 * MinFlowFactorForPumpedHydro)).Disabled = true;
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

