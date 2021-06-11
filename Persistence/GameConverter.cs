namespace SpaceLogistic.Persistence
{
    using System;
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
    using SpaceLogistic.Persistence.Model;
    using SpaceLogistic.Persistence.Model.Celestials;
    using SpaceLogistic.Persistence.Model.Items;
    using SpaceLogistic.Persistence.Model.Routes;
    using SpaceLogistic.Persistence.Model.Ships;
    using SpaceLogistic.Persistence.Model.Stations;
    using SpaceLogistic.Utility;

    public sealed class GameConverter
    {
        private readonly ShipTypes shipTypes;

        private readonly StructureTypes structureTypes;

        private readonly ItemTypes itemTypes;

        private readonly ResourceTypes resourceTypes;

        public GameConverter(ShipTypes shipTypes, StructureTypes structureTypes, ItemTypes itemTypes, ResourceTypes resourceTypes)
        {
            this.shipTypes = shipTypes;
            this.structureTypes = structureTypes;
            this.itemTypes = itemTypes;
            this.resourceTypes = resourceTypes;
        }

        
        public Game Convert(GameData gameData)
        {
            var celestialSystem = this.Convert(gameData.CelestialSystem);
            var locations = celestialSystem.GetLocations().ToDictionary(l => l.Id);
            var routes = gameData.Routes.Select(r => this.Convert(r, locations)).ToDictionary(r => r.Id);
            var ships = gameData.Ships.Select(s => this.Convert(s, locations, routes));
            
            return new Game(
                gameData.Id,
                this.itemTypes,
                this.structureTypes,
                this.shipTypes,
                celestialSystem,
                routes.Values,
                ships);
        }

        private CelestialSystem Convert(CelestialSystemData celestialSystemData)
        {
            return new CelestialSystem(
                celestialSystemData.Id,
                celestialSystemData.Name,
                celestialSystemData.Orbit,
                celestialSystemData.Period,
                celestialSystemData.Color,
                celestialSystemData.Children.Select(this.Convert),
                this.Convert(celestialSystemData.CentralBody),
                celestialSystemData.OrbitalLocations
                    .Select(l => new OrbitalLocation(
                        l.Id,
                        l.Name,
                        l.Orbit,
                        l.Period,
                        this.ConvertStation(l.Colony))));
        }

        private CelestialBody Convert(CelestialBodyData celestialBodyData)
        {
            return new CelestialBody(
                celestialBodyData.Id,
                celestialBodyData.Name,
                celestialBodyData.CelestialBodyType,
                celestialBodyData.Radius,
                celestialBodyData.GravitationalParameter,
                new ResourceCollection(
                    celestialBodyData.Resources.Items.Select(
                        r => new Resource(
                            this.resourceTypes.GetById(r.ResourceType),
                            r.Amount))),
                celestialBodyData.Mass,
                celestialBodyData.Density,
                this.ConvertBase(celestialBodyData.Colony));
        }

        private Base ConvertBase(ColonyData colonyData)
        {
            if (colonyData == null)
            {
                return null;
            }

            var colony = new Base(
                colonyData.Structures
                    .Select(s => new Structure(
                        this.structureTypes.TryGet(s.StructureType, out var structureType) ? structureType : throw new Exception())),
                Enumerable.Empty<ShipConstructionProcess>(), 
                colonyData.Id,
                colonyData.Name,
                colonyData.FuelStorageCapacity,
                colonyData.StoredFuel,
                colonyData.FuelStorageReplenishProcess != null
                    ? new FuelStorageReplenishProcess(
                        colonyData.FuelStorageReplenishProcess.TotalTime,
                        colonyData.FuelStorageReplenishProcess.Progress)
                    : null,
                this.Convert(colonyData.Warehouse));

            foreach (var p in colonyData.ShipConstructionProcesses)
            {
                colony.AddShipConstructionProcess(new ShipConstructionProcess(
                    p.TotalTime, p.Progress, this.shipTypes.TryGet(p.ShipType, out var shipType) ? shipType : throw new Exception(), colony));
            }
            
            return colony;
        }
        
        private Station ConvertStation(ColonyData colonyData)
        {
            if (colonyData == null)
            {
                return null;
            }

            var colony = new Station(
                colonyData.Structures
                    .Select(s => new Structure(
                        this.structureTypes.TryGet(s.StructureType, out var structureType) ? structureType : throw new Exception())),
                Enumerable.Empty<ShipConstructionProcess>(), 
                colonyData.Id,
                colonyData.Name,
                colonyData.FuelStorageCapacity,
                colonyData.StoredFuel,
                colonyData.FuelStorageReplenishProcess != null
                    ? new FuelStorageReplenishProcess(
                        colonyData.FuelStorageReplenishProcess.TotalTime,
                        colonyData.FuelStorageReplenishProcess.Progress)
                    : null,
                this.Convert(colonyData.Warehouse));

            foreach (var p in colonyData.ShipConstructionProcesses)
            {
                colony.AddShipConstructionProcess(new ShipConstructionProcess(
                    p.TotalTime, p.Progress, this.shipTypes.TryGet(p.ShipType, out var shipType) ? shipType : throw new Exception(), colony));
            }
            
            return colony;
        }

        private Ship Convert(
            ShipData shipData, 
            IReadOnlyDictionary<Guid, ILocation> locations,
            IReadOnlyDictionary<Guid, Route> routes)
        {
            var location = locations.GetValueOrDefault(shipData.Location, null);
            var transfer = shipData.Transfer != null
                ? new ShipTransfer(
                    locations[shipData.Transfer.Origin],
                    locations[shipData.Transfer.Destination],
                    shipData.Transfer.TotalTime,
                    shipData.Transfer.Progress)
                : null;
            var refuelProcess = shipData.RefuelingProcess != null
                ? new ShipRefuelingProcess(
                    shipData.RefuelingProcess.TotalTransferredFuel,
                    shipData.RefuelingProcess.RefuelBehavior,
                    shipData.RefuelingProcess.TransferredFuel)
                : null;
            
            return new Ship(
                shipData.Id,
                this.shipTypes.TryGet(shipData.ShipType, out var shipType) ? shipType : throw new Exception(),
                shipData.Name,
                location,
                transfer,
                refuelProcess,
                routes.GetValueOrDefault(shipData.Route, null),
                shipData.Fuel,
                this.Convert(shipData.CargoBay),
                shipData.CargoTransferCapacity);
        }

        private Storage Convert(StorageData storageData)
        {
            return new Storage(
                storageData.Items.Select(
                    i => new Item(
                        this.itemTypes.Get(i.ItemType),
                        i.Amount)));
        }

        private Route Convert(RouteData routeData, IReadOnlyDictionary<Guid, ILocation> locations)
        {
            return new Route(
                routeData.Id,
                routeData.Name,
                routeData.Stops.Select(
                    s => new RouteStop(
                        s.Id,
                        locations[s.Location],
                        s.RefuelBehavior,
                        s.ItemTransferInstructions
                            .Where(i => i.Direction == ItemTransferDirection.Load)
                            .Select(i => new LoadInstruction(
                                this.itemTypes.Get(i.ItemType),
                                i.Amount)),
                        s.ItemTransferInstructions
                            .Where(i => i.Direction == ItemTransferDirection.Unload)
                            .Select(i => new UnloadInstruction(
                                this.itemTypes.Get(i.ItemType),
                                i.Amount)))));
        }
    }
}