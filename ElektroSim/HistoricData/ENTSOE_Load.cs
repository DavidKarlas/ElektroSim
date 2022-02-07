using Sylvan.Data.Csv;

namespace ElektroSim.HistoricData
{
    class ENTSOE_Load
    {
        public double[] Load { get; }
        public double[] DayAheadLoad { get; }
        private ENTSOE_Load(double[] load, double[] dayAheadLoad)
        {
            Load = load;
            DayAheadLoad = dayAheadLoad;
        }

        public static async Task<ENTSOE_Load> ParseAsync(string path, Resolution resolution)
        {
            var load = new double[resolution.NumberOfBrackets];
            var dayAheadLoad = new double[resolution.NumberOfBrackets];
            using var csv = CsvDataReader.Create(path);

            var timeIndex = csv.GetOrdinal("Time (UTC)");
            var dayAheadIndex = csv.GetOrdinal("Day-ahead Total Load Forecast [MW] - Slovenia (SI)");
            var loadIndex = csv.GetOrdinal("Actual Total Load [MW] - Slovenia (SI)");

            int index = 0;
            while (await csv.ReadAsync())
            {
                var loadVal = csv.GetDouble(loadIndex);
                var dayAheadLoadVal = csv.GetDouble(dayAheadIndex);
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
            return new ENTSOE_Load(load, dayAheadLoad);
        }
    }
}
