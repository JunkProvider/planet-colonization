namespace SpaceLogistic.Core.Model.Celestials
{
    using SpaceLogistic.Core.Model.Resources;
    using SpaceLogistic.Core.Model.Stations;

    public interface ILocation : IIdentity
    {
        string Name { get; }

        string FullName { get; }

        Colony Colony { get; }

        ResourceCollection Resources { get; }

        void SetColony(Colony colony);
    }
}
