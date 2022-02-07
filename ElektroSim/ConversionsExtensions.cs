using ElektroSim;
using UnitsNet;

public static class ConversionsExtensions
{
    public static Volume Divide(this VolumeFlow volumeFlow, ResolutionPrecision percision)
    {
        switch (percision)
        {
            case ResolutionPrecision.OneHour:
                return Volume.FromCubicMeters(volumeFlow.CubicMetersPerHour);
            default:
                throw new NotImplementedException();
        }
    }
}