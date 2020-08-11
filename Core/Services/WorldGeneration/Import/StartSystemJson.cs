namespace SpaceLogistic.Core.Services.WorldGeneration.Import
{
    using System.Collections.Generic;

    public sealed class StartSystemJson
    {
        public PlanetJson Star { get; set; }

        public List<PlanetJson> Planets { get; set; }
    }
}