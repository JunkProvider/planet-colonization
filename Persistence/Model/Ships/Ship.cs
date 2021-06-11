namespace SpaceLogistic.Persistence.Model.Ships
{
    using System;
    using SpaceLogistic.Persistence.Model.Items;

    public sealed class ShipData
    {
        public ShipData(Guid id, Guid shipType, string name, Guid location, ShipTransferData transfer, ShipRefuelingProcessData refuelingProcess, Guid route, double fuel, StorageData cargoBay, double cargoTransferCapacity)
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

        public Guid ShipType { get; }

        public string Name { get; set; }

        public Guid Location { get; set; }

        public ShipTransferData Transfer { get; set; }

        public ShipRefuelingProcessData RefuelingProcess { get; set; }
        
        public Guid Route { get; set; }

        public double Fuel { get; set; }

        public StorageData CargoBay { get; }

        public double CargoTransferCapacity { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
