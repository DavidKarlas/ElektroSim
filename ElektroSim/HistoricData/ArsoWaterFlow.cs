using System;
using Sylvan.Data.Csv;

namespace ElektroSim.HistoricData
{
    public class ArsoWaterFlow
    {
        public double[] Data { get; }
        private ArsoWaterFlow(double[] data)
        {
            Data = data;
        }

        public static async Task<ArsoWaterFlow> ParseAsync(string path, Resolution resolution, DateTime start, DateTime end)
        {
            using var csv = CsvDataReader.Create(path);



            while (await csv.ReadAsync())
            {
                var id = csv.GetInt32(0);
                var name = csv.GetString(1);
                var date = csv.GetDateTime(2);
            }
        }
    }
}

