namespace SpaceLogistic.Core.Model.Stations
{
    using SpaceLogistic.Core.Model.Resources;

    public sealed class Station : Colony
    {
        public Station(string name)
            : base(name)
        {
        }

        public override ResourceCollection GetAvailableResources()
        {
            return new ResourceCollection();
        }
    }
}