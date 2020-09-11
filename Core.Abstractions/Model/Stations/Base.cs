﻿namespace SpaceLogistic.Core.Model.Stations
{
    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Core.Model.Resources;

    public sealed class Base : Colony, IIdentity
    {
        public Base(string name)
            : base(name)
        {
        }

        public CelestialBody Location { get; set; }

        public override ResourceCollection GetAvailableResources()
        {
            return this.Location.Resources;
        }
    }
}
