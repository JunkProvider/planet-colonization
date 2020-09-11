namespace SpaceLogistic.Core.Model
{
    using System;

    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Core.Model.Items;
    using SpaceLogistic.Core.Model.ShipRoutes;

    public sealed class Ship : IIdentity
    {
        public Ship(string name, double fuelCapacity, OrbitalLocation location)
        {
            this.Name = name;
            this.FuelCapacity = fuelCapacity;
            this.Location = location;
        }

        public Guid Id { get; } = Guid.NewGuid();

        public string Name { get; }

        public OrbitalLocation Location { get; set; }

        public ShipTransfer Transfer { get; set; }

        public ShipRefuelingProcess RefuelingProcess { get; set; }
        
        public Route Route { get; set; }

        public double FuelCapacity { get; }

        public double Fuel { get; set; }

        public Storage CargoBay { get; } = new Storage();

        public double CargoTransferCapacity { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
