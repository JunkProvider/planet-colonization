namespace SpaceLogistic.Persistence.Model.Stations
{
    using System;

    public sealed class FuelStorageReplenishProcessData : TimedProcessData
    {
        public FuelStorageReplenishProcessData(TimeSpan totalTime, double progress) : base(totalTime, progress)
        {
        }
    }
}
