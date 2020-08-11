namespace SpaceLogistic.Core.Model.Stations
{
    using System;

    public sealed class FuelStorageReplenishProcess : TimedProcess
    {
        public FuelStorageReplenishProcess(TimeSpan totalTime)
            : base(totalTime)
        {
        }
    }
}
