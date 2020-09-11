namespace SpaceLogistic.Core.Model.Stations
{
    using System;
    using System.Collections.Generic;

    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Core.Model.Resources;
    using SpaceLogistic.Core.Model.Structures;

    public sealed class Station : Colony, IStationary, IIdentity
    {
        private readonly List<Structure> structures = new List<Structure>();

        public Station(string name)
            : base(name)
        {
        }

        public OrbitalLocation Location { get; set; }

        public override ResourceCollection GetAvailableResources()
        {
            return new ResourceCollection();
        }
    }
}