namespace SpaceLogistic.Core.Tests
{
    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Core.Services.WorldGeneration.Import;

    public static class TestHelper
    {
        public static CelestialSystem GetSolarSystem()
        {
            var importer = new StarSystemImporter();
            return importer.Import(@"F:\Stuff\Data\solar-system.json").Build();
        }
    }
}
