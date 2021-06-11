namespace SpaceLogistic.Persistence.Model.Stations
{
    using System;
    using System.Collections.Generic;
    using SpaceLogistic.Persistence.Model.Items;
    using SpaceLogistic.Persistence.Model.Structures;

    public class ColonyData
    {
        public ColonyData(Guid id, string name, double fuelStorageCapacity, double storedFuel, FuelStorageReplenishProcessData fuelStorageReplenishProcess, StorageData warehouse, List<StructureData> structures, List<ShipConstructionProcessData> shipConstructionProcesses, Guid location)
        {
            this.Id = id;
            this.Name = name;
            this.FuelStorageCapacity = fuelStorageCapacity;
            this.StoredFuel = storedFuel;
            this.FuelStorageReplenishProcess = fuelStorageReplenishProcess;
            this.Warehouse = warehouse;
            this.Structures = structures;
            this.ShipConstructionProcesses = shipConstructionProcesses;
            this.Location = location;
        }
        
        public Guid Id { get; }
        
        public string Name { get; }

        public double FuelStorageCapacity { get; set; }

        public double StoredFuel { get; set; }

        public FuelStorageReplenishProcessData FuelStorageReplenishProcess { get; set; }

        public StorageData Warehouse { get; }

        public List<StructureData> Structures { get; }

        public List<ShipConstructionProcessData> ShipConstructionProcesses { get; }

        public Guid Location { get; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}