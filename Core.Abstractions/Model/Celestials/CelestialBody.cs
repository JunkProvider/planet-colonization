namespace SpaceLogistic.Core.Model.Celestials
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SpaceLogistic.Core.Model.Resources;
    using SpaceLogistic.Core.Model.Stations;

    public sealed class CelestialBody : ILocation
    {
        public CelestialBody(string name, CelestialBodyType celestialBodyType, double radius, double gravitationalParameter, IEnumerable<Resource> resources)
        {
            this.Name = name;
            this.Radius = radius;
            this.GravitationalParameter = gravitationalParameter;
            this.CelestialBodyType = celestialBodyType;

            this.Resources = new ResourceCollection(resources);

            this.EscapeVelocity = Math.Sqrt((2 * gravitationalParameter) / (radius * 1000));
            this.SurfaceGravity = this.GravitationalParameter / Math.Pow(radius * 1000, 2);
        }
        
        public Guid Id { get; } = Guid.NewGuid();

        public string Name { get; }

        public string FullName => this.GetFullName();

        public double SurfaceRotation { get; } = 0;

        public CelestialBodyType CelestialBodyType { get; }

        public double Radius { get; }

        public double Diameter => this.Radius * 2;

        public double GravitationalParameter { get; }

        public double SurfaceGravity { get; }

        /// <summary>
        /// The escape velocity from the surface of the body.
        /// </summary>
        public double EscapeVelocity { get; }

        public IReadOnlyDictionary<string, double> EscapeVelocities => this.CalculateEscapeVelocities();

        public double Temperature => this.CalculateTemperature();
        
        public ResourceCollection Resources { get; }

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

        public CelestialSystem System { get; internal set; }
        
        public override string ToString()
        {
            return this.Name;
        }

        private IReadOnlyDictionary<string, double> CalculateEscapeVelocities()
        {
            // TODO: pass to ctor
            var escapeVelocities = new Dictionary<string, double>();

            var escapeFromBody = this;
            var escapeFromDistance = this.Radius;
            var escapeToSystem = this.System.Parent;
            var totalEscapeVelocity = 0d;

            while (escapeToSystem != null)
            {
                totalEscapeVelocity += Math.Sqrt((2 * escapeFromBody.GravitationalParameter) / (escapeFromDistance * 1000));
                escapeVelocities.Add(escapeToSystem.Name, totalEscapeVelocity);

                escapeFromBody = escapeToSystem.CentralBody;
                escapeFromDistance = escapeFromBody.System.Orbit;
                escapeToSystem = escapeToSystem.Parent;
            }

            return escapeVelocities;
        }

        private double CalculateTemperature()
        {
            // TODO: pass to ctor
            var distanceFromStar = this.System.GetSelfAndAncestors().Sum(s => s.Orbit);
            var distance = distanceFromStar * 1000;
            var luminosity = 3.846e26;
            var albedo = 0.29;
            var constant = 5.670373e-8;
            var temperature4 = (luminosity * (1d - albedo)) / (16 * Math.PI * Math.Pow(distance, 2) * constant);
            return Math.Pow(temperature4, 1d / 4);
        }

        private string GetFullName()
        {
            if (this.System.IsRoot)
            {
                return this.Name;
            }

            return this.System.GetCentralBodyPathTillRoot();
        }
    }
}