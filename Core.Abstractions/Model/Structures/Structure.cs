namespace SpaceLogistic.Core.Model.Structures
{
    using System;

    public sealed class Structure : IIdentity
    {
        public Structure(StructureType structureType)
        {
            this.StructureType = structureType;
        }

        public Guid Id { get; } = Guid.NewGuid();

        public StructureType StructureType { get; }

        public string Name => this.StructureType.Name;

        public ItemType ProducedItemType => this.StructureType.ProducedItemType;

        public TimeSpan ProductionTime => this.StructureType.ProductionTime;

        public ProductionProcess ProductionProcess { get; set; }
        
        public override string ToString()
        {
            return this.Name;
        }
    }
}