namespace SpaceLogistic.Core.Services
{
    using System;
    using System.Linq;

    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.Items;
    using SpaceLogistic.Core.Model.ShipRoutes;
    using SpaceLogistic.Core.Model.Ships;
    using SpaceLogistic.Core.Model.Stations;
    using SpaceLogistic.Core.Model.Structures;
    using SpaceLogistic.Core.Services.WorldGeneration;

    public sealed class GameUpdateService : IGameUpdateService
    {
        private readonly ITransferCalculator transferCalculator;

        private readonly WorldSettings settings;

        public GameUpdateService(ITransferCalculator transferCalculator, WorldSettings settings)
        {
            this.transferCalculator = transferCalculator;
            this.settings = settings;
        }

        public void Startup(Game game)
        {
        }

        public void Update(Game game, TimeSpan elapsedTime)
        {
            foreach (var ship in game.Ships)
            {
                this.UpdateShip(game, ship, elapsedTime);
            }

            foreach (var colony in game.CelestialSystem.GetColonies())
            {
                foreach (var structure in colony.Structures)
                {
                    this.TryCommandStructureProduction(colony, structure);
                    this.UpdateStructureProduction(colony, structure, elapsedTime);
                }

                this.UpdateColonyFuelStorage(game.ItemTypes, colony, elapsedTime);
                this.UpdateColonyShipConstructions(game, colony, elapsedTime);
            }
        }
        
        private void UpdateShip(Game game, Ship ship, TimeSpan elapsedTime)
        {
            if (ship.Transfer != null)
            {
                this.UpdateShipTransfer(ship, elapsedTime);
                return;
            }

            if (ship.Route == null || ship.Location == null)
            {
                return;
            }

            var currentStop = ship.Route.GetCurrentStop(ship.Location);

            if (currentStop == null)
            {
                return;
            }

            var nextStop = ship.Route.GetNextStop(ship.Location);

            if (nextStop == null || nextStop.Location == currentStop.Location)
            {
                return;
            }

            var requiredFuel = this.GetFuelToTransfer(ship, ship.Route, ship.Route.GetCurrentStop(ship.Location));

            var isDepartureRequirementMet = true;

            var colony = currentStop.Location.Colony;

            if (ship.Fuel < requiredFuel)
            {
                if (colony != null)
                {
                    var maxTransferredFuel = Math.Min(requiredFuel - ship.Fuel, colony.StoredFuel);
                    var transferredFuel = Math.Min(maxTransferredFuel, elapsedTime.TotalSeconds);
                    ship.Fuel += transferredFuel;
                    colony.StoredFuel -= transferredFuel;
                }

                isDepartureRequirementMet = false;
            }

            ship.CargoTransferCapacity = Math.Min(1, ship.CargoTransferCapacity + elapsedTime.TotalSeconds);

            foreach (var instruction in currentStop.UnloadInstructions)
            {
                var amountOnShip = ship.CargoBay.Get(instruction.ItemType);

                if (amountOnShip <= instruction.Amount)
                {
                    continue;
                }

                isDepartureRequirementMet = false;

                if (colony == null || ship.CargoTransferCapacity < 1)
                {
                    break;
                }

                ship.CargoBay.TakeMax(1, instruction.ItemType);
                colony.Warehouse.Add(1, instruction.ItemType);
                ship.CargoTransferCapacity = 0;
            }

            foreach (var instruction in currentStop.LoadInstructions)
            {
                var amountOnShip = ship.CargoBay.Get(instruction.ItemType);

                if (amountOnShip >= instruction.Amount)
                {
                    continue;
                }

                isDepartureRequirementMet = false;

                if (colony == null || ship.CargoTransferCapacity < 1)
                {
                    break;
                }

                var amountOnStation = colony.Warehouse.Get(instruction.ItemType);

                if (amountOnStation < 1)
                {
                    continue;
                }

                colony.Warehouse.TakeMax(1, instruction.ItemType);
                ship.CargoBay.Add(1, instruction.ItemType);
                ship.CargoTransferCapacity = 0;
            }

            if (isDepartureRequirementMet)
            {
                this.TrySendShipToNextDestination(ship);
            }
        }

        private void UpdateShipTransfer(Ship ship, TimeSpan elapsedTime)
        {
            if (ship.Transfer == null)
            {
                return;
            }

            ship.Transfer.UpdateProgress(elapsedTime);

            if (!ship.Transfer.IsCompleted)
            {
                return;
            }

            ship.Location = ship.Transfer.Destination;
            ship.Transfer = null;
        }

        /// <summary>
        /// Gets the amount of fuel to be transferred from the current ship location to the ship so it can depart to the next stop.
        /// </summary>
        private double GetFuelToTransfer(Ship ship, Route route, RouteStop currentStop)
        {
            return Math.Max(0, this.GetFuelRequiredOnShip(ship, route, currentStop));
        }

        /// <summary>
        /// Gets the amount of fuel the ship requires so it can depart to the next stop.
        /// </summary>
        private double GetFuelRequiredOnShip(Ship ship, Route route, RouteStop currentStop)
        {
            switch (currentStop.RefuelBehavior)
            {
                case RefuelBehavior.NoRefuel:
                    return 0;

                case RefuelBehavior.Full:
                    return ship.FuelCapacity;

                case RefuelBehavior.MaxAvailable:
                    return ship.FuelCapacity;

                case RefuelBehavior.MinRequired:
                    return Math.Min(ship.FuelCapacity, this.CalculateMinRequiredFuel(ship, route, currentStop));

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private double CalculateMinRequiredFuel(Ship ship, Route route, RouteStop currentStop)
        {
            var futureStops = route.GetFutureStops(currentStop.Location)
                .ToList();

            var origin = currentStop.Location;

            var totalRequiredFuel = 0d;
            foreach (var futureStop in futureStops)
            {
                // TODO:
                var expectedPayload = 80 * this.settings.ItemMass;

                this.transferCalculator.Calculate(
                    origin,
                    futureStop.Location,
                    ship.EmptyMass + expectedPayload,
                    out var requiredFuel,
                    out _);
                totalRequiredFuel += requiredFuel;

                origin = futureStop.Location;

                if (futureStop.RefuelBehavior == RefuelBehavior.Full || futureStop.RefuelBehavior == RefuelBehavior.MinRequired)
                {
                    break;
                }
            }

            return totalRequiredFuel;
        }

        private void TrySendShipToNextDestination(Ship ship)
        {
            if (ship.Location == null || ship.RefuelingProcess != null || ship.Route == null)
            {
                return;
            }

            var nextDestination = ship.Route.GetNextDestination(ship.Location);

            if (nextDestination == null)
            {
                return;
            }
            
            this.transferCalculator.Calculate(
                ship.Location,
                nextDestination,
                this.settings.ShipEmptyMass + (ship.CargoBay.Items.Count * this.settings.ItemMass),
                out var fuelCosts,
                out var travelTime);

            if (ship.Fuel < fuelCosts)
            {
                return;
            }

            ship.Transfer = new ShipTransfer(
                ship.Location,
                nextDestination,
                travelTime,
                0);

            ship.Location = null;
            ship.Fuel -= fuelCosts;
        }

        private void UpdateStructureProduction(Colony station, Structure structure, TimeSpan elapsedTime)
        {
            if (structure.ProductionProcess == null)
            {
                return;
            }

            structure.ProductionProcess.UpdateProgress(elapsedTime);

            if (!structure.ProductionProcess.IsCompleted)
            {
                return;
            }

            station.Warehouse.Add(1, structure.ProductionProcess.ProducedItemType);
            structure.ProductionProcess = null;
        }

        private void TryCommandStructureProduction(Colony colony, Structure structure)
        {
            if (structure.ProductionProcess != null || structure.ProducedItemType == null)
            {
                return;
            }

            var resources = colony.GetAvailableResources();
            var inventory = colony.Warehouse;

            if (!resources.Contains(structure.ConsumedResources) || !inventory.Contains(structure.ConsumedItems))
            {
                return;
            }

            resources.Take(structure.ConsumedResources);
            inventory.Take(structure.ConsumedItems);
            structure.ProductionProcess = new ProductionProcess(structure.ProducedItemType, structure.ProductionTime);
        }

        private void UpdateColonyFuelStorage(ItemTypes itemTypes, Colony station, TimeSpan elapsedTime)
        {
            if (station.FuelStorageReplenishProcess != null && !station.FuelStorageReplenishProcess.IsCompleted)
            {
                station.FuelStorageReplenishProcess.UpdateProgress(elapsedTime);

                if (station.FuelStorageReplenishProcess.IsCompleted)
                {
                    station.StoredFuel += 1;
                    station.FuelStorageReplenishProcess = null;
                }
            }

            if (station.FuelStorageCapacity <= 0 || station.StoredFuel > (station.FuelStorageCapacity - 1))
            {
                return;
            }

            var fuelAmount = station.Warehouse.TakeMax(1, itemTypes.Fuel);

            if (fuelAmount > 0)
            {
                station.FuelStorageReplenishProcess = new FuelStorageReplenishProcess(TimeSpan.FromSeconds(1), 0);
            }
        }

        private void UpdateColonyShipConstructions(Game game, Colony colony, TimeSpan elapsedTime)
        {
            foreach (var constructionProcess in colony.ShipConstructionProcesses.ToList())
            {
                this.UpdateColonyShipConstruction(game, colony, constructionProcess, elapsedTime);
            }
        }

        private void UpdateColonyShipConstruction(Game game, Colony colony, ShipConstructionProcess constructionProcess, TimeSpan elapsedTime)
        {
            constructionProcess.UpdateProgress(elapsedTime);
            
            if (constructionProcess.IsCompleted)
            {
                this.CompleteColonyShipConstruction(game, colony, constructionProcess);
            }
        }

        private void CompleteColonyShipConstruction(Game game, Colony colony, ShipConstructionProcess constructionProcess)
        {
            var ship = new Ship(constructionProcess.ShipType, $"New {constructionProcess.ShipType.Name}", colony.Location);
            game.AddShip(ship);
            colony.RemoveShipConstructionProcess(constructionProcess);
        }
    }
}
