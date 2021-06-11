namespace SpaceLogistic.Core.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Core.Model.Items;
    using SpaceLogistic.Core.Model.Resources;
    using SpaceLogistic.Core.Model.ShipRoutes;
    using SpaceLogistic.Core.Model.Ships;
    using SpaceLogistic.Core.Model.Stations;
    using SpaceLogistic.Core.Model.Structures;

    public sealed class GameFactory
    {
        private readonly ItemTypes itemTypes;

        private readonly StructureTypes structureTypes;

        private readonly ResourceTypes resourceTypes;

        private readonly ShipTypes shipTypes;

        public GameFactory(ItemTypes itemTypes, StructureTypes structureTypes, ResourceTypes resourceTypes, ShipTypes shipTypes)
        {
            this.itemTypes = itemTypes;
            this.structureTypes = structureTypes;
            this.resourceTypes = resourceTypes;
            this.shipTypes = shipTypes;
        }

        public Game Create(CelestialSystem solarSystem)
        {
            var positions = solarSystem.GetOrbitalLocations().ToList();
            var planets = solarSystem.GetBodies();

            this.AddStation(
                solarSystem, 
                "Low Earth Orbit", 
                "ISS 2",
                new[] { structureTypes.DeuteriumFarm, structureTypes.FuelPlant },
                new[] { new Item(itemTypes.Steel, 10),  });
            this.AddBase(
                solarSystem,
                "Earth",
                "Earth Civilization",
                new[] { structureTypes.DeuteriumFarm, structureTypes.FuelPlant },
                new[] { new Item(itemTypes.Steel, 10), });
            this.AddStation(solarSystem, "Low Mars Orbit", "Mars Station");
            this.AddStation(solarSystem, "Low Io Orbit", "Io Station");
            this.AddStation(solarSystem, "Low Jupiter Orbit", "Jupiter Station");

            var ships = new List<Ship>();
            {
                var ship = new Ship(this.shipTypes.LargeCargoShip, "Magellan", positions.Single(b => b.Name == "Low Earth Orbit"));
                var stop1 = new RouteStop(
                    positions.Single(b => b.Name == "Low Earth Orbit"), 
                    RefuelBehavior.MinRequired,
                    new[] { new LoadInstruction(itemTypes.Deuterium, 5),  },
                    Enumerable.Empty<UnloadInstruction>());
                var stop2 = new RouteStop(
                    positions.Single(b => b.Name == "Low Mars Orbit"), 
                    RefuelBehavior.NoRefuel,
                    Enumerable.Empty<LoadInstruction>(),
                    new[] { new UnloadInstruction(itemTypes.Deuterium, 0),  });
                // ship.Route = new Route("Earth <-> Mars", new[] { stop1, stop2 });
                ships.Add(ship);
            }

            {
                var ship = new Ship(this.shipTypes.SmallCargoShip, "Orion", positions.Single(b => b.Name == "Low Earth Orbit"));
                ships.Add(ship);
            }

            return new Game(itemTypes, structureTypes, solarSystem, ships, this.shipTypes);
        }

        private void AddStation(CelestialSystem startSystem, string location, string name)
        {
            this.AddStation(startSystem, location, name, Enumerable.Empty<StructureType>(), Enumerable.Empty<Item>());
        }

        private void AddStation(
            CelestialSystem startSystem, 
            string location, 
            string name, 
            IEnumerable<StructureType> structures,
            IEnumerable<Item> items)
        {
            var station = new Station(name);
            
            foreach (var structureType in structures)
            {
                station.AddStructure(new Structure(structureType));
            }

            station.FuelStorageCapacity = 50;
            station.StoredFuel = 25;
            station.Warehouse.Add(items);
            startSystem.GetOrbitalLocationWithName(location).SetColony(station);
        }

        private void AddBase(
            CelestialSystem startSystem,
            string location,
            string name,
            IEnumerable<StructureType> structures,
            IEnumerable<Item> items)
        {
            var @base = new Base(name);

            foreach (var structureType in structures)
            {
                @base.AddStructure(new Structure(structureType));
            }

            @base.FuelStorageCapacity = 50;
            @base.StoredFuel = 25;
            @base.Warehouse.Add(items);
            startSystem.GetBodyWithName(location).SetColony(@base);
        }
    }
}
