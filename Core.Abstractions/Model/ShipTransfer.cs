namespace SpaceLogistic.Core.Model
{
    using System;

    using SpaceLogistic.Core.Model.Celestials;

    public sealed class ShipTransfer : TimedProcess
    {
        public ShipTransfer(OrbitalLocation origin, OrbitalLocation destination, TimeSpan totalTime)
            : base(totalTime)
        {
            if (origin == destination)
            {
                throw new ArgumentException("Origin and destination can not be the same.");
            }

            this.Origin = origin ?? throw new ArgumentNullException(nameof(origin));
            this.Destination = destination ?? throw new ArgumentNullException(nameof(destination));
        }

        public OrbitalLocation Origin { get; }

        public OrbitalLocation Destination { get; }
    }
}