namespace SpaceLogistic.Core.Services.WorldGeneration
{
    public interface IWorldGenerator
    {
        CelestialBodyBuilder Modify(CelestialBodyBuilder star);
    }
}