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
            : this(
                Guid.NewGuid(), 
                name,
                orbit,
                period,
                color,
                children,
                centralBody,
                CreateOrbitalLocations(centralBody, lowOrbit))
        {
        }

        public CelestialSystem(Guid id, string name, double orbit, TimeSpan period, Color color, IEnumerable<CelestialSystem> children, CelestialBody centralBody, IEnumerable<OrbitalLocation> orbitalLocations)
        {
            this.Id = id;
            this.Name = name;
            this.Orbit = orbit;
            this.Period = period;
            this.Color = color;
            this.Children = children.ToList();
            this.CentralBody = centralBody;
            this.OrbitalLocations = orbitalLocations.ToList();
            
            foreach (var child in this.Children)
            {
                child.Parent = this;
            }
            
            this.CentralBody.System = this;
            
            foreach (var orbitalPosition in this.OrbitalLocations)
            {
                orbitalPosition.System = this;
            }
        }

        public Guid Id { get; }

        public string Name { get; }

        public double Orbit { get; }

        public TimeSpan Period { get; }

        public Color Color { get; }

        public CelestialSystem Parent { get; internal set; }

        public bool IsRoot => this.Parent == null;

        public IReadOnlyCollection<CelestialSystem> Children { get; }

        public CelestialBody CentralBody { get; }

        public IReadOnlyCollection<OrbitalLocation> OrbitalLocations { get; }

        public OrbitalLocation LowOrbit => this.OrbitalLocations.First();
        
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

        public bool TryGetLocation(Guid locationId, out ILocation location)
        {
            location = this.GetLocations().FirstOrDefault(l => l.Id == locationId);
            return location != null;
        }

        public IEnumerable<ILocation> GetLocations()
        {
            return Enumerable.Repeat<ILocation>(this.CentralBody, 1)
                .Concat(this.OrbitalLocations)
                .Concat(this.Children.SelectMany(c => c.GetLocations()));
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

        public IEnumerable<Colony> GetColonies()
        {
            return this.GetOwnColonies().Concat(this.Children.SelectMany(c => c.GetColonies()));
        }

        public IEnumerable<Colony> GetOwnColonies()
        {
            if (this.CentralBody.Colony != null)
            {
                yield return this.CentralBody.Colony;
            }

            foreach (var orbitalLocation in this.OrbitalLocations)
            {
                if (orbitalLocation.Colony != null)
                {
                    yield return orbitalLocation.Colony;
                }
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public CelestialBody GetBodyWithName(string name)
        {
            return this.GetBodies().First(b => b.Name == name);
        }

        private static IEnumerable<OrbitalLocation> CreateOrbitalLocations(CelestialBody centralBody, double lowOrbit)
        {
            var lowOrbitPeriod = TimeSpan.FromSeconds(2 * Math.PI * Math.Sqrt(Math.Pow((centralBody.Radius + lowOrbit), 3) / centralBody.GravitationalParameter));

            yield return new OrbitalLocation(
                Guid.NewGuid(), 
                $"Low {centralBody.Name} Orbit",
                centralBody.Radius + lowOrbit,
                lowOrbitPeriod,
                null);
        }
    }
}