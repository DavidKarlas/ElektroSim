// See https://aka.ms/new-console-template for more information
using ElektroSim;
using ElektroSim.HistoricData;

var resolution = new Resolution(ResolutionPrecision.OneHour,
                    new DateTime(2020, 1, 1),
                    new DateTime(2021, 1, 1));


var eNTSOE_Data = await ENTSOE_Data.ParseAsync(resolution);
var solarMaxPower = await SolarMaxPower.ParseAsync(resolution);

var sloveniaElectroSystem = new Slovenia(eNTSOE_Data, solarMaxPower);
var arsoWaterFlowData = await ArsoWaterFlow.ParseAsync(resolution, sloveniaElectroSystem.WaterBodies.SelectMany(w => w.InFlows).Distinct().ToArray());
var simulator = new Simulator(resolution, sloveniaElectroSystem, eNTSOE_Data, arsoWaterFlowData);

simulator.Run();