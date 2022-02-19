using System;
using Sylvan.Data.Csv;
using UnitsNet;

namespace ElektroSim.HistoricData
{
    public class SolarMaxPower
    {
        public Power[] SolarMaxPowerArray { get; }

        public SolarMaxPower(Power[] solarMaxPower)
        {
            SolarMaxPowerArray = solarMaxPower;
        }


        public static async Task<SolarMaxPower> ParseAsync(Resolution resolution)
        {
            var solarMaxPower = new Power[resolution.NumberOfBrackets];

            using var csv = CsvDataReader.Create(await DownloadAndCache.GetFileAsync("https://davidupload.blob.core.windows.net/data/SonceTotals.csv"));

            var yearIndex = csv.GetOrdinal("Year");
            var totalPowerAtEndOfYearIndex = csv.GetOrdinal("Total");

            int index = 0;
            double previousTotal = 0;
            while (await csv.ReadAsync())
            {
                var year = csv.GetInt32(yearIndex);
                var totalPowerAtEndOfYear = csv.GetDouble(totalPowerAtEndOfYearIndex);
                if (year<resolution.Start.Year|| year > resolution.End.Year)
                {
                    previousTotal = totalPowerAtEndOfYear;
                    continue;
                }

                var time = resolution.GetTime(index);
                var daysInYear = DateTime.IsLeapYear(year) ? 366 : 365;
                for (int i = 0; i < daysInYear; i++)
                {
                    var powerOnDay = Power.FromMegawatts(previousTotal + (totalPowerAtEndOfYear - previousTotal) * (i / daysInYear));
                    for (int j = 0; j < 24; j++)
                    {
                        solarMaxPower[index++] = powerOnDay;
                    }
                }
                previousTotal = totalPowerAtEndOfYear;
            }
            if (index != resolution.NumberOfBrackets)
                throw new Exception();
            return new SolarMaxPower(solarMaxPower);
        }
    }
}

