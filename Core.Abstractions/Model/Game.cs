namespace SpaceLogistic.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Core.Model.Items;
    using SpaceLogistic.Core.Model.ShipRoutes;
    using SpaceLogistic.Core.Model.Stations;
    using SpaceLogistic.Core.Model.Structures;
    using SpaceLogistic.Utility;

    public sealed class Game
    {
        private readonly List<Route> routes;

        public Game(ItemTypes itemTypes, StructureTypes structureTypes, CelestialSystem celestialSystem, IEnumerable<Ship> ships)
        {
            this.ItemTypes = itemTypes;
            this.StructureTypes = structureTypes;
            this.CelestialSystem = celestialSystem;
            this.Ships = ships.ToList();

            this.routes = this.Ships.Select(s => s.Route).Where(r => r != null).Distinct().ToList();
        }

        public ItemTypes ItemTypes { get; }

        public StructureTypes StructureTypes { get; }

        public CelestialSystem CelestialSystem { get; }

        public IReadOnlyCollection<CelestialBody> Bodies => this.CelestialSystem.GetBodies().ToList();

        public IReadOnlyCollection<Ship> Ships { get; }

        public IReadOnlyCollection<Route> Routes => this.routes;

        public bool TryGetColony(Guid id, out Colony colony)
        {
            colony = this.GetColonyOrDefault(id);
            return colony != null;
        }

        public Colony GetColonyOrDefault(Guid? id)
        {
            return id.HasValue ? this.GetColonyOrDefault(id.Value) : null;
        }

        public Colony GetColonyOrDefault(Guid id)
        {
            return this.CelestialSystem.GetAllColonies().FirstOrDefault(c => c.Id == id);
        }

        public void AddRoute(Route route)
        {
            this.routes.Add(route);
        }

        public void DeleteRoute(Guid routeId)
        {
            this.routes.RemoveWhere(r => r.Id == routeId);

            foreach (var ship in this.Ships)
            {
                if (ship.Route?.Id == routeId)
                {
                    ship.Route = null;
                }
            }
        }
    }
}
