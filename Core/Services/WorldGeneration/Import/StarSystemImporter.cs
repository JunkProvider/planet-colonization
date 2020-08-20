namespace SpaceLogistic.Core.Services.WorldGeneration.Import
{
    using System.IO;

    using JPV.RocketScience;

    using Newtonsoft.Json;

    public sealed class StarSystemImporter : IStarSystemImporter
    {
        public CelestialBodyBuilder Import(string filePath)
        {
            var starSystemJson = JsonConvert.DeserializeObject<StartSystemJson>(File.ReadAllText(filePath));
            var starJson = starSystemJson.Star;

            return new CelestialBodyBuilder(Physics.GravitationalConstant)
                .WithName(starSystemJson.Star.Name)
                .WithDiameter(starSystemJson.Star.Diameter)
                .WithMass(starJson.Mass)
                .WithChildren(starSystemJson.Planets, (planetJson, planet) => planet
                    .WithName(planetJson.Name)
                    .WithOrbit(planetJson.Orbit * 1e6)
                    .WithDiameter(planetJson.Diameter)
                    .WithMass(planetJson.Mass)
                    .WithChildren(planetJson.Moons, (moonJson, moon) => moon
                        .WithName(moonJson.Name)
                        .WithOrbit(moonJson.Orbit * 1e3)
                        .WithDiameter(moonJson.Diameter)
                        .WithMass(moonJson.Mass)));
        }
    }
}
