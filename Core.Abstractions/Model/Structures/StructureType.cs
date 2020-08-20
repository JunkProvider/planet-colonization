namespace SpaceLogistic.Core.Model.Structures
{
    using System;

    public sealed class StructureType
    {
        public StructureType(string name, ItemType producedItemType, TimeSpan productionTime)
        {
            this.Name = name;
            this.ProducedItemType = producedItemType;
            this.ProductionTime = productionTime;
        }

        public string Name { get; }

        public ItemType ProducedItemType { get; }

        public TimeSpan ProductionTime { get; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
