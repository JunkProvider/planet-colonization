namespace SpaceLogistic.Core.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Core.Model.Items;
    using SpaceLogistic.Core.Model.ShipRoutes;
    using SpaceLogistic.Core.Model.Stations;
    using SpaceLogistic.Core.Model.Structures;

    public sealed class GameFactory
    {
        private readonly CelestialSystem world;

        public GameFactory(CelestialSystem world)
        {
            this.world = world;
        }

        public Game Create()
        {
            var itemTypes = new ItemTypes();
            var structureTypes = new StructureTypes(itemTypes);

            var solarSystem = this.world;

            var positions = solarSystem.GetOrbitalLocations().ToList();
            var planets = solarSystem.GetBodies();

            this.AddStation(solarSystem, "Low Earth Orbit", "ISS 2", new[] { structureTypes.DeuteriumFarm, structureTypes.FuelPlant });
            this.AddStation(solarSystem, "Low Mars Orbit", "Mars Station");
            this.AddStation(solarSystem, "Low Io Orbit", "Io Station");
            this.AddStation(solarSystem, "Low Jupiter Orbit", "Jupiter Station");

            var ships = new List<Ship>();
            {
                var ship = new Ship("Magellan", 20,  positions.Single(b => b.Name == "Low Earth Orbit"));
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
                var ship = new Ship("Columbus", 15, positions.Single(b => b.Name == "Low Earth Orbit"));
                ships.Add(ship);
            }

            return new Game(itemTypes, structureTypes, solarSystem, ships);
        }

        private void AddStation(CelestialSystem startSystem, string location, string name)
        {
            this.AddStation(startSystem, location, name, Enumerable.Empty<StructureType>());
        }

        private void AddStation(CelestialSystem startSystem, string location, string name, IEnumerable<StructureType> structures)
        {
            var station = new Station(name);
            
            foreach (var structureType in structures)
            {
                station.AddStructure(new Structure(structureType));
            }

            station.FuelStorageCapacity = 50;
            station.StoredFuel = 25;
            startSystem.GetOrbitalLocationWithName(location).SetObject(station);
        }
    }
}
