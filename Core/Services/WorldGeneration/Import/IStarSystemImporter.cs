namespace SpaceLogistic.Core.Services.WorldGeneration.Import
{
    public interface IStarSystemImporter
    {
        CelestialBodyBuilder Import(string filePath);
    }
}