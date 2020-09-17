namespace SpaceLogistic.Core.Model.Stations
{
    using SpaceLogistic.Core.Model.Resources;

    public sealed class Base : Colony
    {
        public Base(string name)
            : base(name)
        {
        }

        public override ResourceCollection GetAvailableResources()
        {
            return this.Location.Resources;
        }
    }
}
