namespace SpaceLogistic.Persistence
{
    using System;
    using System.Linq;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Core.Model.Items;
    using SpaceLogistic.Core.Model.ShipRoutes;
    using SpaceLogistic.Core.Model.Ships;
    using SpaceLogistic.Core.Model.Stations;
    using SpaceLogistic.Persistence.Model;
    using SpaceLogistic.Persistence.Model.Celestials;
    using SpaceLogistic.Persistence.Model.Items;
    using SpaceLogistic.Persistence.Model.Resources;
    using SpaceLogistic.Persistence.Model.Routes;
    using SpaceLogistic.Persistence.Model.Ships;
    using SpaceLogistic.Persistence.Model.Stations;
    using SpaceLogistic.Persistence.Model.Structures;

    public sealed class GameDataConverter
    {
        public GameData Convert(Game game)
        {
            return new GameData(
                game.Id,
                this.Convert(game.CelestialSystem),
                game.Ships
                    .Select(this.Convert)
                    .ToList(),
                game.Routes
                    .Select(this.Convert)
                    .ToList());
        }

        private CelestialSystemData Convert(CelestialSystem celestialSystem)
        {
            return new CelestialSystemData(
                celestialSystem.Id,
                celestialSystem.Name,
                celestialSystem.Orbit,
                celestialSystem.Period,
                celestialSystem.Color,
                celestialSystem.Children
                    .Select(this.Convert)
                    .ToList(),
                this.Convert(celestialSystem.CentralBody),
                celestialSystem.OrbitalLocations
                    .Select(l => new OrbitalLocationData(
                        l.Id,
                        l.Name,
                        l.Orbit,
                        l.Period,
                        l.Colony.ConvertNullable(this.Convert)))
                    .ToList());
        }

        private CelestialBodyData Convert(CelestialBody celestialBody)
        {
            return new CelestialBodyData(
                celestialBody.Id,
                celestialBody.Name,
                celestialBody.CelestialBodyType,
                celestialBody.Radius,
                celestialBody.Mass,
                celestialBody.Density,
                celestialBody.GravitationalParameter,
                new ResourceCollectionData(
                    celestialBody.Resources.Items.Select(i => new ResourceData(i.ResourceType.Id, i.Amount)).ToList()),
                celestialBody.Colony.ConvertNullable(this.Convert));
        }

        private ColonyData Convert(Colony colony)
        {
            return new ColonyData(
                colony.Id,
                colony.Name,
                colony.FuelStorageCapacity,
                colony.StoredFuel,
                colony.FuelStorageReplenishProcess
                    .ConvertNullable(p => new FuelStorageReplenishProcessData(colony.FuelStorageReplenishProcess.TotalTime, colony.FuelStorageReplenishProcess.Progress)),
                this.Convert(colony.Warehouse),
                colony.Structures
                    .Select(s => new StructureData(
                        s.Id,
                        s.StructureType.Id,
                        s.ProductionProcess
                            .ConvertNullable(p => new ProductionProcessData(s.ProductionProcess.TotalTime, s.ProductionProcess.Progress, s.ProducedItemType.Id))))
                    .ToList(),
                colony.ShipConstructionProcesses
                    .Select(p => new ShipConstructionProcessData(p.TotalTime, p.Progress, p.ShipType.Id))
                    .ToList(),
                colony.Location.Id);
        }

        private ShipData Convert(Ship ship)
        {
            return new ShipData(
                ship.Id,
                ship.ShipType.Id,
                ship.Name,
                ship.Location?.Id ?? Guid.Empty,
                ship.Transfer.ConvertNullable(
                    t => new ShipTransferData(
                        t.TotalTime,
                        t.Progress,
                        t.Origin.Id,
                        t.Destination.Id)),
                ship.RefuelingProcess.ConvertNullable(
                    p => new ShipRefuelingProcessData(
                        p.TotalTransferredFuel,
                        p.RefuelBehavior,
                        p.TransferredFuel)),
                ship.Route?.Id ?? Guid.Empty,
                ship.Fuel,
                this.Convert(ship.CargoBay),
                ship.CargoTransferCapacity);
        }

        private RouteData Convert(Route route)
        {
            return new RouteData(
                route.Id,
                route.Name,
                route.Stops
                    .Select(s => new RouteStopData(
                        s.Id,
                        s.Location.Id,
                        s.RefuelBehavior,
                        s.LoadInstructions
                            .Select(i => new ItemTransferInstructionData(
                                i.Id,
                                i.ItemType.Id,
                                i.Amount,
                                ItemTransferDirection.Load))
                            .Concat(s.UnloadInstructions
                                .Select(i => new ItemTransferInstructionData(
                                    i.Id,
                                    i.ItemType.Id,
                                    i.Amount,
                                    ItemTransferDirection.Unload)))
                            .ToList()))
                    .ToList());
        }

        private StorageData Convert(Storage storage)
        {
            return new StorageData(storage.Items.Select(i => new ItemData(i.ItemType.Id, i.Amount)).ToList());
        }
    }
}