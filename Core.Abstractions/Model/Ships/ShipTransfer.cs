namespace SpaceLogistic.Core.Model
{
    using System;

    using SpaceLogistic.Core.Model.Celestials;

    public sealed class ShipTransfer : TimedProcess
    {
        public ShipTransfer(ILocation origin, ILocation destination, TimeSpan totalTime, double progress)
            : base(totalTime, progress)
        {
            if (origin == destination)
            {
                throw new ArgumentException("Origin and destination can not be the same.");
            }

            this.Origin = origin ?? throw new ArgumentNullException(nameof(origin));
            this.Destination = destination ?? throw new ArgumentNullException(nameof(destination));
        }

        public ILocation Origin { get; }

        public ILocation Destination { get; }
    }
}