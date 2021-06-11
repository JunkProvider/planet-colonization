namespace SpaceLogistic.Persistence.Model.Stations
{
    using System;

    public sealed class ShipConstructionProcessData : TimedProcessData
    {
        public ShipConstructionProcessData(TimeSpan totalTime, double progress, Guid shipType) : base(totalTime, progress)
        {
            this.ShipType = shipType;
        }

        public Guid ShipType { get; }
    }
}
