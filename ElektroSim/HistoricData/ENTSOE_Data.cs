using Sylvan.Data.Csv;

namespace ElektroSim.HistoricData
{
    public class ENTSOE_Data
    {
        public double[] Load { get; }
        public double[] DayAheadLoad { get; }
        public double[] SolarProduction { get; }

        private ENTSOE_Data(double[] load, double[] dayAheadLoad, double[] solarProduction)
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

        private static async Task<(double[] load, double[] dayAheadLoad)> ParseLoadAsync(Resolution resolution)
        {
            var load = new double[resolution.NumberOfBrackets];
            var dayAheadLoad = new double[resolution.NumberOfBrackets];
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
                    var dataTime = DateTime.Parse(csv.GetString(timeIndex).Remove(16));
                    var resTime = resolution.GetTime(index);
                    if (dataTime != resTime)
                        throw new Exception();
                    if (resolution.Precision == ResolutionPrecision.OneHour)
                    {
                        load[index] = loadVal;
                        dayAheadLoad[index++] = dayAheadLoadVal;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }

            return (load, dayAheadLoad);
        }

        private static async Task<double[]> ParseGenerationAsync(Resolution resolution)
        {
            var solarProduction = new double[resolution.NumberOfBrackets];
            for (int year = resolution.Start.Year; year < resolution.End.Year; year++)
            {
                using var csv = CsvDataReader.Create(await DownloadAndCache.GetFileAsync($"https://davidupload.blob.core.windows.net/data/Generation_{year}.csv"));

                var mtuIndex = csv.GetOrdinal("MTU");
                var solarIndex = csv.GetOrdinal("Solar  - Actual Aggregated [MW]");

                int index = 0;
                while (await csv.ReadAsync())
                {
                    var time = resolution.GetTime(index);
                    var parsedTime = DateTime.Parse(csv.GetString(mtuIndex).Remove(16));

                    if (time != parsedTime)
                        throw new Exception();

                    var solar = csv.GetDouble(solarIndex);
                    if (resolution.Precision == ResolutionPrecision.OneHour)
                    {
                        solarProduction[index++] = solar;
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
