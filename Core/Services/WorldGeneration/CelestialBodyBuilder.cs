namespace SpaceLogistic.Core.Services.WorldGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using JPV.RocketScience;

    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Core.Model.Resources;
    using SpaceLogistic.Utility;

    public sealed class CelestialBodyBuilder
    {
        private readonly CelestialBodyBuilder parent;

        private readonly List<CelestialBodyBuilder> children = new List<CelestialBodyBuilder>();

        private string systemName;

        public CelestialBodyBuilder(double gravitationalConstant)
        {
            this.GravitationalConstant = gravitationalConstant;
        }

        public CelestialBodyBuilder(CelestialBodyBuilder parent)
        {
            this.parent = parent;
            this.GravitationalConstant = parent.GravitationalConstant;
            this.MinRelevantGravity = parent.MinRelevantGravity;
        }
        
        public string Name { get; private set; } = "Unnamed";

        public CelestialBodyType CelestialBodyType { get; private set; }

        public string SystemName => this.systemName ?? $"{this.Name} System";

        public double GravitationalConstant { get; private set; }

        /// <summary>
        /// In km
        /// </summary>
        public double Orbit { get; private set; }

        public TimeSpan OrbitalPeriod => this.parent != null
            ? TimeSpan.FromSeconds(2 * Math.PI * Math.Sqrt(Math.Pow(this.Orbit, 3) / this.parent.GravitationalParameter))
            : TimeSpan.Zero;

        /// <summary>
        /// In km
        /// </summary>
        public double Radius { get; private set; }

        /// <summary>
        /// In km
        /// </summary>
        public double Diameter => this.Radius * 2;

        /// <summary>
        /// In kg
        /// </summary>
        public double Mass { get; private set; }

        /// <summary>
        /// In kg/m³
        /// </summary>
        public double Density => Physics.GetSphereDensity(this.Radius, this.Mass) * 1000;

        public double GravitationalParameter => Physics.GetStandardGravitationalParameter(this.Mass, this.GravitationalConstant);

        /// <summary>
        /// In m/s²
        /// </summary>
        public double SurfaceGravity => this.GravitationalParameter / (this.Radius * this.Radius);

        /// <summary>
        /// In km
        /// </summary>
        public double SphereOfInfluence => this.parent != null ? this.Orbit * Math.Pow(this.Mass / this.parent.Mass, 2d / 5) : double.PositiveInfinity;

        /// <summary>
        /// In m/s²
        /// </summary>
        public double MinRelevantGravity { get; private set; } = 0.01;

        /// <summary>
        /// In km
        /// </summary>
        public double SphereOfRelevance => Math.Sqrt(this.GravitationalParameter / this.MinRelevantGravity);

        /// <summary>
        /// In km
        /// </summary>
        public double SystemRadius
        {
            get
            {
                var lastChild = this.Children.LastOrDefault();
                if (lastChild == null)
                {
                    return this.Radius;
                }

                return lastChild.Orbit + lastChild.Radius;
            }
        }

        public Color Color { get; private set; } = Color.Gray;

        public IReadOnlyCollection<Resource> Resources { get; private set; } = new List<Resource>();

        public IEnumerable<CelestialBodyBuilder> All => Enumerable.Repeat(this, 1).Concat(this.Descendants);

        public IEnumerable<CelestialBodyBuilder> Children => this.children.OrderBy(c => c.Orbit);

        public IEnumerable<CelestialBodyBuilder> Descendants => this.Children.Concat(this.Children.SelectMany(c => c.Descendants));

        public double GetMinDistanceBetweenBodies()
        {
            var lowestDistance = double.PositiveInfinity;
            var prevHighFace = this.Radius;
            
            foreach (var child in this.Children)
            {
                var radius = child.SystemRadius;
                var lowFace = child.Orbit - radius;
                var distance = lowFace - prevHighFace;

                if (distance < lowestDistance)
                {
                    lowestDistance = distance;
                }

                prevHighFace = child.Orbit + radius;
            }

            return Enumerable.Repeat(lowestDistance, 1)
                .Concat(this.children.Select(c => c.GetMinDistanceBetweenBodies()))
                .Min();
        }

        public CelestialBodyBuilder WithName(string name)
        {
            this.Name = name;
            return this;
        }

        public CelestialBodyBuilder WithSystemName(string systemNameParam)
        {
            this.systemName = systemNameParam;
            return this;
        }

        public CelestialBodyBuilder WithGravitationalConstant(double gravitationalConstant)
        {
            this.GravitationalConstant = gravitationalConstant;
            return this;
        }

        public CelestialBodyBuilder WithOrbit(double orbit)
        {
            this.Orbit = orbit;
            return this;
        }

        public CelestialBodyBuilder WithRadius(double radius)
        {
            this.Radius = radius;
            return this;
        }

        public CelestialBodyBuilder WithDiameter(double diameter)
        {
            this.Radius = diameter / 2;
            return this;
        }

        public CelestialBodyBuilder WithMass(double mass)
        {
            this.Mass = mass;
            return this;
        }

        public CelestialBodyBuilder WithDensity(double density)
        {
            this.Mass = Physics.GetSphereMass(this.Radius, density);
            return this;
        }

        public CelestialBodyBuilder WithMinRelevantGravity(double minRelevantGravity)
        {
            this.MinRelevantGravity = minRelevantGravity;
            return this;
        }

        public void WithColor(Color color)
        {
            this.Color = color;
        }

        public void WithResources(IEnumerable<Resource> resources)
        {
            this.Resources = resources.ToList();
        }

        public CelestialBodyBuilder WithChildren<TInput>(IEnumerable<TInput> inputs, Action<TInput, CelestialBodyBuilder> buildChild)
        {
            foreach (var input in inputs)
            {
                this.WithChild(child => buildChild(input, child));
            }

            return this;
        }

        public CelestialBodyBuilder WithChild(Action<CelestialBodyBuilder> buildChild)
        {
            var child = new CelestialBodyBuilder(this);

            switch (this.CelestialBodyType)
            {
                case CelestialBodyType.Star:
                    child.CelestialBodyType = CelestialBodyType.Planet;
                    break;
                case CelestialBodyType.Planet:
                    child.CelestialBodyType = CelestialBodyType.Moon;
                    break;
                case CelestialBodyType.Moon:
                    child.CelestialBodyType = CelestialBodyType.Moon;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this.children.Add(child);
            buildChild(child);
            return this;
        }

        public CelestialBodyBuilder ForAll(Action<CelestialBodyBuilder> action)
        {
            action(this);

            foreach (var child in this.children)
            {
                child.ForAll(action);
            }

            return this;
        }

        public CelestialSystem Build()
        {
            double lowOrbit;

            switch (this.CelestialBodyType)
            {
                case CelestialBodyType.Star:
                    lowOrbit = 10000e3;
                    break;
                case CelestialBodyType.Planet:
                    lowOrbit = 200e3;
                    break;
                case CelestialBodyType.Moon:
                    lowOrbit = 100e3;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var period = this.parent == null || this.Orbit <= 0
                ? TimeSpan.Zero
                : TimeSpan.FromSeconds(2 * Math.PI * Math.Sqrt(Math.Pow(this.Orbit, 3) / this.parent.GravitationalParameter));

            return new CelestialSystem(
                this.SystemName,
                this.Orbit,
                period,
                new CelestialBody(
                    this.Name,
                    this.CelestialBodyType,
                    this.Radius,
                    this.GravitationalParameter,
                    this.Resources,
                    this.Mass,
                    this.Density),
                lowOrbit,
                this.children.Select(c => c.Build()),
                this.Color);
        }

        public void Print(int indent = 0)
        {
            var sor = this.SphereOfRelevance;
            var soi = this.SphereOfInfluence;

            Out.WriteLine(indent, this.Name, ConsoleColor.White);
            Out.WriteLine(indent, $"Orbit:      {(this.Orbit / 1e6):0.00} Gm");
            Out.WriteLine(indent, $"SOI:        {(soi / 1e6):0.00} Gm", soi < this.SystemRadius * 1.1 ? ConsoleColor.Red : ConsoleColor.Gray);
            Out.WriteLine(indent, $"Period:     {(this.OrbitalPeriod.TotalDays):0} Days");
            Out.WriteLine(indent, $"Gravity:    {(this.SurfaceGravity):0.00} m/s²");
            

            /*Out.WriteLine(indent, $"Sys Rad:    {((this.SystemRadius) / 1e6):0.000} Gm");
            Out.WriteLine(indent, $"SOR:        {(sor / 1e6):0.000} Gm", sor < this.SystemRadius ? ConsoleColor.Red : ConsoleColor.Gray);
            
            Out.WriteLine(indent, $"Diameter:   {(this.Diameter / 1e3):0.0} Mm");
            Out.WriteLine(indent, $"Density:    {this.Density:0.0} kg/m³");
            Out.WriteLine(indent, $"Surf. Grav: {this.SurfaceGravity:0.00} m/s²");*/
            
            foreach (var child in this.Children)
            {
                Out.WriteLine();
                child.Print(indent + 3);
            }
        }
    }
}
