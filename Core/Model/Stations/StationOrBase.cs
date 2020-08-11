namespace SpaceLogistic.Core.Model.Stations
{
    using System;
    using System.Collections.Generic;

    using SpaceLogistic.Core.Model.Structures;

    public abstract class StationOrBase : IIdentity
    {
        private readonly List<Structure> structures = new List<Structure>();

        protected StationOrBase(string name)
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

        public void AddStructure(Structure structure)
        {
            this.structures.Add(structure);
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}