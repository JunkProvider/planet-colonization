namespace SpaceLogistic.Core.Model.Ships
{
    using System;

    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Core.Model.Items;
    using SpaceLogistic.Core.Model.ShipRoutes;

    public sealed class Ship : IIdentity
    {
        public Ship(ShipType shipType, string name, ILocation location)
        {
            ShipType = shipType;
            this.Name = name;
            this.Location = location;
        }

        public Guid Id { get; } = Guid.NewGuid();

        public ShipType ShipType { get; }

        public string Name { get; }

        public ILocation Location { get; set; }

        public ShipTransfer Transfer { get; set; }

        public ShipRefuelingProcess RefuelingProcess { get; set; }
        
        public Route Route { get; set; }

        public double EmptyMass => this.ShipType.EmptyMass;

        public double FuelCapacity => this.ShipType.FuelCapacity;

        public double Fuel { get; set; }

        public Storage CargoBay { get; } = new Storage();

        public double CargoTransferCapacity { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
