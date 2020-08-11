namespace SpaceLogistic.Core.Services.WorldGeneration.Import
{
    using System.Collections.Generic;

    public sealed class PlanetJson
    {
        public string Name { get; set; }

        public double Orbit { get; set; }

        public double Diameter { get; set; }

        public double Mass { get; set; }

        public List<PlanetJson> Moons { get; set; } = new List<PlanetJson>();
    }
}