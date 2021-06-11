namespace SpaceLogistic.Core.Model.Ships
{
    using System;

    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Core.Model.Items;
    using SpaceLogistic.Core.Model.ShipRoutes;

    public sealed class Ship : IIdentity
    {
        public Ship(ShipType shipType, string name, ILocation location)
            : this(
                Guid.NewGuid(),
                shipType,
                name,
                location,
                null,
                null,
                null,
                0,
                new Storage(),
                0)
        {
        }

        public Ship(Guid id, ShipType shipType, string name, ILocation location, ShipTransfer transfer, ShipRefuelingProcess refuelingProcess, Route route, double fuel, Storage cargoBay, double cargoTransferCapacity)
        {
            this.Id = id;
            this.ShipType = shipType;
            this.Name = name;
            this.Location = location;
            this.Transfer = transfer;
            this.RefuelingProcess = refuelingProcess;
            this.Route = route;
            this.Fuel = fuel;
            this.CargoBay = cargoBay;
            this.CargoTransferCapacity = cargoTransferCapacity;
        }

        public Guid Id { get; }

        public ShipType ShipType { get; }

        public string Name { get; set; }

        public ILocation Location { get; set; }

        public ShipTransfer Transfer { get; set; }

        public ShipRefuelingProcess RefuelingProcess { get; set; }
        
        public Route Route { get; set; }

        public double EmptyMass => this.ShipType.EmptyMass;

        public double FuelCapacity => this.ShipType.FuelCapacity;

        public double Fuel { get; set; }

        public Storage CargoBay { get; }

        public double CargoTransferCapacity { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
