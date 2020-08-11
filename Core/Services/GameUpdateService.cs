namespace SpaceLogistic.Core.Services
{
    using System;
    using System.Linq;

    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.Items;
    using SpaceLogistic.Core.Model.ShipRoutes;
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
            foreach (var ship in game.Ships)
            {
                if (ship.Location != null)
                {
                    // this.OnShipArrived(ship);
                }
            }
        }

        public void Update(Game game, TimeSpan elapsedTime)
        {
            foreach (var ship in game.Ships)
            {
                this.UpdateShip(game, ship, elapsedTime);
            }

            foreach (var location in game.CelestialSystem.GetOrbitalLocations())
            {
                if (!(location.Object is Station station))
                {
                    continue;
                }

                foreach (var structure in station.Structures)
                {
                    this.UpdateStructureCommand(station, structure);
                    this.UpdateStructureProduction(station, structure, elapsedTime);
                }

                this.UpdateColonyFuelStorage(game.ItemTypes, station, elapsedTime);
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

            var station = currentStop.Location.Object as Station;

            if (ship.Fuel < requiredFuel)
            {
                if (station != null)
                {
                    var maxTransferredFuel = Math.Min(requiredFuel - ship.Fuel, station.StoredFuel);
                    var transferredFuel = Math.Min(maxTransferredFuel, elapsedTime.TotalSeconds);
                    ship.Fuel += transferredFuel;
                    station.StoredFuel -= transferredFuel;
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

                if (station == null || ship.CargoTransferCapacity < 1)
                {
                    break;
                }

                ship.CargoBay.TakeMax(1, instruction.ItemType);
                station.Warehouse.Add(1, instruction.ItemType);
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

                if (station == null || ship.CargoTransferCapacity < 1)
                {
                    break;
                }

                var amountOnStation = station.Warehouse.Get(instruction.ItemType);

                if (amountOnStation < 1)
                {
                    continue;
                }

                station.Warehouse.TakeMax(1, instruction.ItemType);
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

            // this.OnShipArrived(ship);
        }

        private void OnShipArrived(Ship ship)
        {
            if (ship.Route == null || ship.Location == null)
            {
                return;
            }

            var stop = ship.Route.GetCurrentStop(ship.Location);

            if (stop == null)
            {
                return;
            }

            var station = ship.Location.Object as Station;
            
            foreach (var unloadInstruction in stop.UnloadInstructions)
            {
                station.Warehouse.Add(ship.CargoBay.TakeMax(unloadInstruction.Amount, unloadInstruction.ItemType), unloadInstruction.ItemType);
            }

            foreach (var loadInstruction in stop.LoadInstructions)
            {
                ship.CargoBay.Add(station.Warehouse.TakeMax(loadInstruction.Amount, loadInstruction.ItemType), loadInstruction.ItemType);
            }

            this.StartRefuelingShip(ship);
        }

        private void StartRefuelingShip(Ship ship)
        {
            var currentStop = ship.Route?.GetCurrentStop(ship.Location);

            if (currentStop == null || currentStop.RefuelBehavior == RefuelBehavior.NoRefuel)
            {
                return;
            }

            var transferredFuel = this.GetFuelToTransfer(ship, ship.Route, currentStop);

            if (transferredFuel <= 0)
            {
                return;
            }

            ship.RefuelingProcess = new ShipRefuelingProcess(transferredFuel, currentStop.RefuelBehavior);
        }

        private void UpdateShipRefuelProgress(Ship ship, TimeSpan elapsedTime)
        {
            if (ship.RefuelingProcess == null)
            {
                return;
            }

            var fuelStation = ship.Location.Object as Station;

            var transferredFuelRate = elapsedTime.TotalSeconds * 2.5;
            var transferredFuel = Math.Min(transferredFuelRate, Math.Min(ship.RefuelingProcess.RemainingTransferredFuel, fuelStation.StoredFuel));

            ship.RefuelingProcess.UpdateProgress(transferredFuel);

            ship.Fuel += transferredFuel;
            fuelStation.StoredFuel -= transferredFuel;

            if (ship.RefuelingProcess.IsCompleted)
            {
                ship.RefuelingProcess = null;
                return;
            }

            if (ship.RefuelingProcess.RefuelBehavior == RefuelBehavior.MaxAvailable && fuelStation.StoredFuel <= 0)
            {
                ship.RefuelingProcess = null;
            }
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
                    return Math.Min(ship.FuelCapacity, this.CalculateMinRequiredFuel(route, currentStop));

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private double CalculateMinRequiredFuel(Route route, RouteStop currentStop)
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
                    this.settings.ShipEmptyMass + expectedPayload,
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
                travelTime);

            ship.Location = null;
            ship.Fuel -= fuelCosts;
        }

        private void UpdateStructureProduction(Station station, Structure structure, TimeSpan elapsedTime)
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

        private void UpdateStructureCommand(Station station, Structure structure)
        {
            if (structure.ProductionProcess != null || structure.ProducedItemType == null)
            {
                return;
            }

            structure.ProductionProcess = new ProductionProcess(structure.ProducedItemType, structure.ProductionTime);
        }

        private void UpdateColonyFuelStorage(ItemTypes itemTypes, Station station, TimeSpan elapsedTime)
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
                station.FuelStorageReplenishProcess = new FuelStorageReplenishProcess(TimeSpan.FromSeconds(1));
            }
        }
    }
}
