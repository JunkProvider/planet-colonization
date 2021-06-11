namespace SpaceLogistic.Core.Model.Stations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Core.Model.Items;
    using SpaceLogistic.Core.Model.Resources;
    using SpaceLogistic.Core.Model.Structures;

    public abstract class Colony : IIdentity
    {
        private readonly List<Structure> structures;

        private readonly HashSet<ShipConstructionProcess> shipConstructionProcesses;

        protected Colony(string name)
            : this(
                Enumerable.Empty<Structure>(),
                Enumerable.Empty<ShipConstructionProcess>(),
                Guid.NewGuid(),
                name,
                0,
                0,
                null,
                new Storage())
        {
        }

        protected Colony(
            IEnumerable<Structure> structures,
            IEnumerable<ShipConstructionProcess> shipConstructionProcesses,
            Guid id,
            string name,
            double fuelStorageCapacity,
            double storedFuel,
            FuelStorageReplenishProcess fuelStorageReplenishProcess,
            Storage warehouse)
        {
            this.structures = structures.ToList();
            this.shipConstructionProcesses = new HashSet<ShipConstructionProcess>(shipConstructionProcesses);
            this.Id = id;
            this.Name = name;
            this.FuelStorageCapacity = fuelStorageCapacity;
            this.StoredFuel = storedFuel;
            this.FuelStorageReplenishProcess = fuelStorageReplenishProcess;
            this.Warehouse = warehouse;
        }

        public Guid Id { get; }
        
        public string Name { get; }

        public double FuelStorageCapacity { get; set; }

        public double StoredFuel { get; set; }

        public FuelStorageReplenishProcess FuelStorageReplenishProcess { get; set; }

        public Storage Warehouse { get; }

        public IReadOnlyCollection<Structure> Structures => this.structures;

        public IReadOnlyCollection<ShipConstructionProcess> ShipConstructionProcesses => this.shipConstructionProcesses;

        public ILocation Location { get; private set; }

        public abstract ResourceCollection GetAvailableResources();

        public bool TryGetStructure(Guid structureId, out Structure structure)
        {
            structure = this.structures.FirstOrDefault(s => s.Id == structureId);
            return structure != null;
        }

        public void AddStructure(Structure structure)
        {
            this.structures.Add(structure);
        }

        public void RemoveStructure(Guid structureId)
        {
            this.structures.RemoveAll(s => s.Id == structureId);
        }

        public void SetLocation(ILocation location)
        {
            if (this.Location == location)
            {
                return;
            }

            this.Location?.SetColony(null);
            this.Location = location;
            location.SetColony(this);
        }

        public void AddShipConstructionProcess(ShipConstructionProcess shipConstructionProcess)
        {
            this.shipConstructionProcesses.Add(shipConstructionProcess);
        }

        public void RemoveShipConstructionProcess(ShipConstructionProcess shipConstructionProcess)
        {
            this.shipConstructionProcesses.Remove(shipConstructionProcess);
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}