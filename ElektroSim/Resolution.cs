using System;
namespace ElektroSim
{
    public enum Resolution
    {
        OneHour,
        OneDay
    }

    public static class ResolutionHelper
    {
        public static int CalculateBrackets(Resolution resolution, DateTime start, DateTime end)
        {
            switch (resolution)
            {
                case Resolution.OneHour:
                    return (int)(end - start).TotalHours;
                case Resolution.OneDay:
                    return (int)(end - start).TotalDays;
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}

