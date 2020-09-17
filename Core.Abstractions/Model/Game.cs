namespace SpaceLogistic.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Core.Model.Items;
    using SpaceLogistic.Core.Model.ShipRoutes;
    using SpaceLogistic.Core.Model.Ships;
    using SpaceLogistic.Core.Model.Stations;
    using SpaceLogistic.Core.Model.Structures;
    using SpaceLogistic.Utility;

    public sealed class Game
    {
        private readonly List<Route> routes;

        private readonly HashSet<Ship> ships;

        public Game(ItemTypes itemTypes, StructureTypes structureTypes, CelestialSystem celestialSystem, IEnumerable<Ship> ships, ShipTypes shipTypes)
        {
            this.ItemTypes = itemTypes;
            this.StructureTypes = structureTypes;
            this.CelestialSystem = celestialSystem;
            ShipTypes = shipTypes;
            this.ships = ships.ToSet();

            this.routes = this.Ships.Select(s => s.Route).Where(r => r != null).Distinct().ToList();
        }

        public ItemTypes ItemTypes { get; }

        public StructureTypes StructureTypes { get; }

        public ShipTypes ShipTypes { get; }

        public CelestialSystem CelestialSystem { get; }

        public IReadOnlyCollection<Ship> Ships => this.ships;

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
            return this.CelestialSystem.GetColonies().FirstOrDefault(c => c.Id == id);
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

        public IReadOnlyCollection<Ship> GetShipsAtLocation(ILocation location)
        {
            return this.ships.Where(s => s.Location?.Id == location?.Id).ToList();
        }

        public void AddShip(Ship ship)
        {
            this.ships.Add(ship);
        }
    }
}
