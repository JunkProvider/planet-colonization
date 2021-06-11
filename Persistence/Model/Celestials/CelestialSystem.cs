namespace SpaceLogistic.Persistence.Model.Celestials
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public sealed class CelestialSystemData
    {
        public CelestialSystemData(Guid id, string name, double orbit, TimeSpan period, Color color, List<CelestialSystemData> children, CelestialBodyData centralBody, List<OrbitalLocationData> orbitalLocations)
        {
            this.Id = id;
            this.Name = name;
            this.Orbit = orbit;
            this.Period = period;
            this.Color = color;
            this.Children = children;
            this.CentralBody = centralBody;
            this.OrbitalLocations = orbitalLocations;
        }
        
        public Guid Id { get; }

        public string Name { get; }

        public double Orbit { get; }

        public TimeSpan Period { get; }

        public Color Color { get; }

        public List<CelestialSystemData> Children { get; }

        public CelestialBodyData CentralBody { get; }

        public List<OrbitalLocationData> OrbitalLocations { get; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}