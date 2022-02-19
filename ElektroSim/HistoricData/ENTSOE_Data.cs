using System.Globalization;
using Sylvan.Data.Csv;
using UnitsNet;

namespace ElektroSim.HistoricData
{
    public class ENTSOE_Data
    {
        public Power[] Load { get; }
        public Power[] DayAheadLoad { get; }
        public Power[] SolarProduction { get; }

        private ENTSOE_Data(Power[] load, Power[] dayAheadLoad, Power[] solarProduction)
        {
            Load = load;
            DayAheadLoad = dayAheadLoad;
            SolarProduction = solarProduction;
        }

        public static async Task<ENTSOE_Data> ParseAsync(Resolution resolution)
        {
            var solarProduction = await ParseGenerationAsync(resolution);
            var (load, dayAheadLoad) = await ParseLoadAsync(resolution);
            return new ENTSOE_Data(load, dayAheadLoad, solarProduction);
        }

        private static async Task<(Power[] load, Power[] dayAheadLoad)> ParseLoadAsync(Resolution resolution)
        {
            var load = new Power[resolution.NumberOfBrackets];
            var dayAheadLoad = new Power[resolution.NumberOfBrackets];
            for (int year = resolution.Start.Year; year < resolution.End.Year; year++)
            {
                using var csv = CsvDataReader.Create(await DownloadAndCache.GetFileAsync($"https://davidupload.blob.core.windows.net/data/Load_{year}.csv"));

                var timeIndex = csv.GetOrdinal("Time (UTC)");
                var dayAheadIndex = csv.GetOrdinal("Day-ahead Total Load Forecast [MW] - BZN|SI");
                var loadIndex = csv.GetOrdinal("Actual Total Load [MW] - BZN|SI");

                int index = 0;
                while (await csv.ReadAsync())
                {
                    var loadVal = csv.GetDouble(loadIndex);
                    var dayAheadLoadVal = csv.GetDouble(dayAheadIndex);
                    var parsedTime = DateTime.ParseExact(csv.GetString(timeIndex).Remove(16), "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
                    var resTime = resolution.GetTime(index);
                    if (parsedTime != resTime)
                        throw new Exception();
                    if (resolution.Precision == ResolutionPrecision.OneHour)
                    {
                        load[index] = Power.FromMegawatts(loadVal);
                        dayAheadLoad[index++] = Power.FromMegawatts(dayAheadLoadVal);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }

            return (load, dayAheadLoad);
        }

        private static async Task<Power[]> ParseGenerationAsync(Resolution resolution)
        {
            var solarProduction = new Power[resolution.NumberOfBrackets];
            for (int year = resolution.Start.Year; year < resolution.End.Year; year++)
            {
                using var csv = CsvDataReader.Create(await DownloadAndCache.GetFileAsync($"https://davidupload.blob.core.windows.net/data/Generation_{year}.csv"));

                var mtuIndex = csv.GetOrdinal("MTU");
                var solarIndex = csv.GetOrdinal("Solar  - Actual Aggregated [MW]");

                int index = 0;
                while (await csv.ReadAsync())
                {
                    var time = resolution.GetTime(index);
                    var parsedTime = DateTime.ParseExact(csv.GetString(mtuIndex).Remove(16), "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);

                    if (time != parsedTime)
                        throw new Exception();

                    var solar = csv.GetDouble(solarIndex);
                    if (resolution.Precision == ResolutionPrecision.OneHour)
                    {
                        solarProduction[index++] = Power.FromMegawatts(solar);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }

            return solarProduction;
        }
    }
}
