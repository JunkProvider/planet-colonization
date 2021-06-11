namespace SpaceLogistic.Persistence.Model.Ships
{
    using System;

    public sealed class ShipTransferData : TimedProcessData
    {
        public ShipTransferData(TimeSpan totalTime, double progress, Guid origin, Guid destination) : base(totalTime, progress)
        {
            this.Origin = origin;
            this.Destination = destination;
        }

        public Guid Origin { get; }

        public Guid Destination { get; }
    }
}