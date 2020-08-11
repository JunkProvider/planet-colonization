namespace SpaceLogistic.Core.Model
{
    using SpaceLogistic.Core.Model.Celestials;

    public interface IStationary
    {
        string Name { get; }

        OrbitalLocation Location { get; set; }
    }
}