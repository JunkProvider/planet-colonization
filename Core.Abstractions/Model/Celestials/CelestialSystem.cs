namespace SpaceLogistic.Core.Model.Celestials
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using SpaceLogistic.Core.Model.Stations;

    public sealed class CelestialSystem : IIdentity
    {
        public CelestialSystem(string name, double orbit, TimeSpan period, CelestialBody centralBody, double lowOrbit, Color color)
            : this(name, orbit, period, centralBody, lowOrbit, Enumerable.Empty<CelestialSystem>(), color)
        {
        }

        public CelestialSystem(
            string name,
            double orbit,
            TimeSpan period,
            CelestialBody centralBody,
            double lowOrbit,
            IEnumerable<CelestialSystem> children,
            Color color)
        {
            this.Name = name;
            this.Orbit = orbit;
            this.Children = children.ToList();
            this.CentralBody = centralBody;
            this.Color = color;
            this.Period = period;

            foreach (var child in this.Children)
            {
                child.Parent = this;
            }

            this.CentralBody.System = this;
            
            var lowOrbitPeriod = TimeSpan.FromSeconds(2 * Math.PI * Math.Sqrt(Math.Pow((centralBody.Radius + lowOrbit) * 1000, 3) / this.CentralBody.GravitationalParameter));

            this.OrbitalLocations = new[]
                {
                    new OrbitalLocation(
                        $"Low {this.CentralBody.Name} Orbit", 
                        centralBody.Radius + lowOrbit,
                        lowOrbitPeriod),
                };

            foreach (var orbitalPosition in this.OrbitalLocations)
            {
                orbitalPosition.System = this;
            }
        }

        public Guid Id { get; } = Guid.NewGuid();

        public string Name { get; }

        public double Orbit { get; }

        public TimeSpan Period { get; }

        public Color Color { get; }

        public CelestialSystem Parent { get; internal set; }

        public IReadOnlyCollection<CelestialSystem> Children { get; }

        public CelestialBody CentralBody { get; }

        public IReadOnlyCollection<OrbitalLocation> OrbitalLocations { get; }
        
        public static CelestialSystem GetFirstCommonAncestorOrSelf(CelestialSystem celestialSystemA, CelestialSystem celestialSystemB)
        {
            var aAncestors = celestialSystemA.GetSelfAndAncestors().Reverse().ToArray();
            var bAncestors = celestialSystemB.GetSelfAndAncestors().Reverse().ToArray();

            var length = Math.Min(aAncestors.Length, bAncestors.Length);

            if (length == 0)
            {
                throw new InvalidOperationException($"{celestialSystemA} and {celestialSystemB} have no common ancestor.");
            }

            var i = 0;
            while (i < length && aAncestors[i] == bAncestors[i])
            {
                i++;
            }

            if (i == 0)
            {
                throw new InvalidOperationException($"{celestialSystemA} and {celestialSystemB} have no common ancestor.");
            }

            return aAncestors[i - 1];
        }

        public CelestialSystem GetById(Guid systemId)
        {
            return this.GetSelfAndDescendants().FirstOrDefault(s => s.Id == systemId)
                ?? throw new InvalidOperationException($"System with id {systemId} not found.");
        }

        public IEnumerable<CelestialBody> GetBodies()
        {
            return Enumerable.Repeat(this.CentralBody, 1).Concat(this.Children.SelectMany(c => c.GetBodies()));
        }

        public OrbitalLocation GetOrbitalLocationWithName(string name)
        {
            var orbitalLocation = this.GetOrbitalLocations().SingleOrDefault(l => l.Name == name);

            if (orbitalLocation == null)
            {
                var orbitalLocationNames = string.Join(", ", this.GetOrbitalLocations().Select(l => l.Name));
                throw new Exception($"Could not find orbital location with name {name}. Available orbital locations are: {orbitalLocationNames}.");
            }

            return orbitalLocation;
        }

        public IEnumerable<OrbitalLocation> GetOrbitalLocations()
        {
            return this.OrbitalLocations.Concat(this.Children.SelectMany(c => c.GetOrbitalLocations()));
        }

        public string GetCentralBodyPathTillRoot()
        {
            if (this.Parent == null)
            {
                return string.Empty;
            }

            var parentPath = this.Parent.GetCentralBodyPathTillRoot();

            if (!string.IsNullOrEmpty(parentPath))
            {
                return $"{parentPath} > {this.CentralBody.Name}";
            }

            return this.CentralBody.Name;
        }

        public IEnumerable<Colony> GetAllColonies()
        {
            return this.GetOwnColonies().Concat(this.Children.SelectMany(c => c.GetAllColonies()));
        }

        public IEnumerable<Colony> GetOwnColonies()
        {
            if (this.CentralBody.Base != null)
            {
                yield return this.CentralBody.Base;
            }

            foreach (var orbitalLocation in this.OrbitalLocations)
            {
                if (orbitalLocation.Object is Station station)
                {
                    yield return station;
                }
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}