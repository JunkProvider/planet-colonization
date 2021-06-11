namespace SpaceLogistic.Persistence.Model
{
    using System;
    using System.Collections.Generic;
    using SpaceLogistic.Persistence.Model.Celestials;
    using SpaceLogistic.Persistence.Model.Routes;
    using SpaceLogistic.Persistence.Model.Ships;

    public sealed class GameData
    {
        public GameData(Guid id, CelestialSystemData celestialSystem, IReadOnlyCollection<ShipData> ships, IReadOnlyCollection<RouteData> routes)
        {
            this.Id = id;
            this.CelestialSystem = celestialSystem;
            this.Ships = ships;
            this.Routes = routes;
        }
        
        public Guid Id { get; }

        public CelestialSystemData CelestialSystem { get; }

        public IReadOnlyCollection<ShipData> Ships { get; }

        public IReadOnlyCollection<RouteData> Routes { get; }
    }
}
