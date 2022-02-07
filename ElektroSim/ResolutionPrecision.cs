using System;
namespace ElektroSim
{
    public enum ResolutionPrecision
    {
        OneHour,
        OneDay
    }

    public class Resolution
    {
        public ResolutionPrecision Precision { get; }
        public DateTime Start { get; }
        public DateTime End { get; }
        public int NumberOfBrackets { get; }

        public Resolution(ResolutionPrecision precision, DateTime start, DateTime end)
        {
            Precision = precision;
            Start = start;
            End = end;
            NumberOfBrackets = CalculateBrackets(precision, start, end);
        }


        private static int CalculateBrackets(ResolutionPrecision resolution, DateTime start, DateTime end)
        {
            switch (resolution)
            {
                case ResolutionPrecision.OneHour:
                    return (int)(end - start).TotalHours;
                case ResolutionPrecision.OneDay:
                    return (int)(end - start).TotalDays;
                default:
                    throw new NotSupportedException();
            }
        }

        public DateTime GetTime(int i)
        {
            switch (Precision)
            {
                case ResolutionPrecision.OneHour:
                    return Start.AddHours(i);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}

