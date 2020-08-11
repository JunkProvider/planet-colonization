namespace SpaceLogistic.Core.Model.Stations
{
    using SpaceLogistic.Core.Model.Celestials;

    public sealed class Base : StationOrBase, IIdentity
    {
        public Base(string name)
            : base(name)
        {
        }

        public CelestialBody Location { get; set; }
    }
}
