using System;
using System.Globalization;
using Sylvan.Data.Csv;
using UnitsNet;

namespace ElektroSim.HistoricData
{
    public class ArsoWaterFlow
    {
        public Dictionary<int, Volume[]> Inflows { get; } = new();

        private ArsoWaterFlow(Dictionary<int, Volume[]> inflows)
        {
            Inflows = inflows;
        }

        public static async Task<ArsoWaterFlow> ParseAsync(Resolution resolution, params int[] ids)
        {
            var inflows = new Dictionary<int, Volume[]>();
            foreach (var id in ids.Append(3080).Append(3180))
            {
                if (id == 3060)
                    continue;//We need to fake this... Data is bad...
                var path = await DownloadAndCache.GetFileAsync($"http://vode.arso.gov.si/hidarhiv/pov_arhiv_tab.php?p_vodotok=Drava&p_postaja={id}&p_od_leto=2015&p_do_leto=2020&b_oddo_CSV=Izvoz+dnevnih+vrednosti+v+CSV",
                    true);
                using var csv = CsvDataReader.Create(path);

                var datumIndex = csv.GetOrdinal("Datum");
                var pretokIndex = csv.GetOrdinal("pretok (m3/s)");
                var data = new Volume[resolution.NumberOfBrackets];
                inflows.Add(id, data);

                int index = 0;
                while (await csv.ReadAsync())
                {
                    var parsedTime = DateOnly.ParseExact(csv.GetString(datumIndex), "dd.MM.yyyy");
                    if (parsedTime.Year < resolution.Start.Year)
                        continue;
                    if (parsedTime.Year > resolution.End.Year)
                        break;
                    var time = DateOnly.FromDateTime(resolution.GetTime(index));

                    if (time != parsedTime)
                        throw new Exception();
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

                if (index != resolution.NumberOfBrackets)
                    throw new Exception();
            }

            var fake = new Volume[resolution.NumberOfBrackets];
            var podMoste = inflows[3080];
            var radovna = inflows[3180];
            for (int i = 0; i < fake.Length; i++)
            {
                fake[i] = podMoste[i] - radovna[i];
            }
            inflows[3060] = fake;
            return new ArsoWaterFlow(inflows);
        }
    }
}

