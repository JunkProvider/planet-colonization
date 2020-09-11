namespace SpaceLogistic.Core.Services.WorldGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;

    using SpaceLogistic.Core.Model.Resources;

    public sealed class WorldGenerator : IWorldGenerator
    {
        private readonly WorldSettings settings;

        private readonly ResourceTypes resourceTypes;

        private readonly Dictionary<string, Color> colors = new Dictionary<string, Color>
                                                                {
                                                                    { "Sun", Color.Yellow },
                                                                    { "Mercury", Color.Gray },
                                                                    { "Venus", HexColor("D1BE47") },
                                                                    { "Earth", HexColor("4577A0") },
                                                                    { "Mars", HexColor("E48158") },
                                                                    { "Jupiter", HexColor("C7BD9A") },
                                                                    { "Saturn", HexColor("F2D1B0") },
                                                                    { "Uranus", HexColor("B2D7DD") },
                                                                    { "Neptune", HexColor("3955D3") },
                                                                    { "Io", HexColor("F6E269") },
                                                                };

        private readonly Dictionary<string, IEnumerable<(string, int)>> resources = new Dictionary<string, IEnumerable<(string, int)>>
            {
                { "Earth", new[] { ("Water", 5000), ("Deuterium", 100), ("Iron", 350) } },
                { "Mars", new[] { ("Water", 100), ("Iron", 250) } },
                { "Luna", new[] { ("Iron", 100) } },

                // Io has the least amount of water of any known body in the Solar System.
                // Most volcanic.
                // Full of sulfur and silicates.
                { "Io", new[] { ("Iron", 100) } },

                { "Europa", new[] { ("Water", 5000), ("Deuterium", 100) } }
            };

        public WorldGenerator(WorldSettings settings, ResourceTypes resourceTypes)
        {
            this.settings = settings;
            this.resourceTypes = resourceTypes;
        }

        public CelestialBodyBuilder Modify(CelestialBodyBuilder star)
        {
            return star
                .ForAll(b => b.WithGravitationalConstant(settings.GravitationalConstant))
                .ForAll(b => b.WithOrbit(b.Orbit * settings.OrbitFactor))
                .ForAll(b => b.WithRadius(b.Radius * settings.SizeFactor))
                .ForAll(b => b.WithColor(this.GetColor(b.Name)))
                .ForAll(b => b.WithResources(this.GetResources(b.Name)))
                /*.ForAll(b => b.WithDensity(b.Density * settings.DensityFactor))*/;
        }

        private Color GetColor(string planetName)
        {
            if (this.colors.TryGetValue(planetName, out var color))
            {
                return color;
            }

            return Color.Gray;
        }
        
        private static Color HexColor(string hex)
        {
            var r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
            var g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
            var b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
            return Color.FromArgb(r, g, b);
        }

        private IEnumerable<Resource> GetResources(string planetName)
        {
            if (this.resources.TryGetValue(planetName, out var resource))
            {
                return resource.Select(r => new Resource(
                    this.resourceTypes.GetByName(r.Item1), r.Item2));
            }

            return Enumerable.Empty<Resource>();
        }
    }
}
