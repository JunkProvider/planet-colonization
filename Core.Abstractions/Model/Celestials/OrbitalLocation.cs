﻿namespace SpaceLogistic.Core.Model.Celestials
{
    using System;

    using SpaceLogistic.Core.Model.Resources;
    using SpaceLogistic.Core.Model.Stations;

    public sealed class OrbitalLocation : ILocation, IIdentity
    {
        public OrbitalLocation(Guid id, string name, double orbit, TimeSpan period, Colony colony)
        {
            this.Id = id;
            this.Name = name;
            this.Orbit = orbit;
            this.Period = period;
            this.SetColony(colony);
        }

        public Guid Id { get; }

        public string Name { get; }

        public string FullName => this.GetFullName();

        public double Orbit { get; }

        public TimeSpan Period { get; }

        public ResourceCollection Resources { get; } = new ResourceCollection();

        public CelestialSystem System { get; internal set; }

        public Colony Colony { get; private set; }
        
        public void SetColony(Colony colony)
        {
            if (colony == this.Colony)
            {
                return;
            }

            this.Colony?.SetLocation(null);
            this.Colony = colony;
            colony.SetLocation(this);
        }

        public override string ToString()
        {
            if (this.Colony == null)
            {
                return $"{this.Name}";
            }

            return $"{this.Name} - {this.Colony}";
        }


        private string GetFullName()
        {
            var parentName = this.System.GetCentralBodyPathTillRoot();

            if (string.IsNullOrEmpty(parentName))
            {
                return this.Name;
            }

            return $"{parentName} > {this.Name}";
        }
    }
}
