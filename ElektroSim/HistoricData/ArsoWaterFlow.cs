using System;
using Sylvan.Data.Csv;
using UnitsNet;

namespace ElektroSim.HistoricData
{
    public class ArsoWaterFlow
    {
        public Dictionary<string, Volume[]> NamedInflows { get; } = new();

        private ArsoWaterFlow(Dictionary<string, Volume[]> namedInflows)
        {
            NamedInflows = namedInflows;
        }

        public static async Task<ArsoWaterFlow> ParseAsync(string[] paths, Resolution resolution)
        {
            var namedInflows = new Dictionary<string, Volume[]>();
            foreach (var path in paths)
            {
                using var csv = CsvDataReader.Create(path);

                var fileName = Path.GetFileNameWithoutExtension(path);
                var pretokIndex = csv.GetOrdinal("pretok (m3/s)");
                var data = new Volume[resolution.NumberOfBrackets];
                namedInflows.Add(fileName, data);

                int index = 0;
                while (await csv.ReadAsync())
                {
                    var val = csv.GetDouble(pretokIndex);
                    if (resolution.Precision == ResolutionPrecision.OneHour)
                    {
                        for (int j = 0; j < 24; j++)
                        {
                            data[index++] = Volume.FromCubicMeters(val * 3600);
                        }
                    }
                    else
                    {
                        data[index++] = Volume.FromCubicMeters(val * 3600 * 24);
                    }
                }
            }
            return new ArsoWaterFlow(namedInflows);
        }
    }
}

