// See https://aka.ms/new-console-template for more information
using ElektroSim;
using ElektroSim.HistoricData;

var resolution = new Resolution(ResolutionPrecision.OneHour,
                    new DateTime(2020, 1, 1),
                    new DateTime(2021, 1, 1));

var arsoWaterFlowData = await ArsoWaterFlow.ParseAsync(
                    new[] { @"C:\Users\davkar\Downloads\Avstrijska meja.txt" }, resolution);

var ENTSOE_LoadData = await ENTSOE_Load.ParseAsync(
                     @"C:\Users\davkar\Downloads\Total Load - Day Ahead _ Actual_202001010000-202101010000.csv", resolution);

var dravaRiver = new Slovenia();

var simulator = new Simulator(resolution, dravaRiver, ENTSOE_LoadData, arsoWaterFlowData);

simulator.Run();

Console.ReadLine();