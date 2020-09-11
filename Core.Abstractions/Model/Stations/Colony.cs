namespace SpaceLogistic.Core.Model.Stations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SpaceLogistic.Core.Model.Items;
    using SpaceLogistic.Core.Model.Resources;
    using SpaceLogistic.Core.Model.Structures;

    public abstract class Colony : IIdentity
    {
        private readonly List<Structure> structures = new List<Structure>();

        protected Colony(string name)
        {
            this.Name = name;
        }

        public Guid Id { get; } = Guid.NewGuid();

        public string Name { get; }

        public double FuelStorageCapacity { get; set; }

        public double StoredFuel { get; set; }

        public FuelStorageReplenishProcess FuelStorageReplenishProcess { get; set; }

        public Storage Warehouse { get; } = new Storage();

        public IReadOnlyCollection<Structure> Structures => this.structures;

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

        public override string ToString()
        {
            return this.Name;
        }
    }
}