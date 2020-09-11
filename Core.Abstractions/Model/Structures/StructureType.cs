namespace SpaceLogistic.Core.Model.Structures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SpaceLogistic.Core.Model.Items;
    using SpaceLogistic.Core.Model.Resources;

    public sealed class StructureType : IIdentity
    {
        public StructureType(
            Guid id,
            string name,
            string description,
            IEnumerable<Resource> consumedResources,
            ItemType producedItemType,
            TimeSpan productionTime,
            IEnumerable<Item> constructionMaterials)
            : this(id, name, description, consumedResources, Enumerable.Empty<Item>(), producedItemType, productionTime, constructionMaterials)
        {
        }

        public StructureType(
            Guid id,
            string name,
            string description,
            IEnumerable<Item> consumedItems,
            ItemType producedItemType,
            TimeSpan productionTime,
            IEnumerable<Item> constructionMaterials)
            : this(id, name, description, Enumerable.Empty<Resource>(), consumedItems, producedItemType, productionTime, constructionMaterials)
        {
        }

        public StructureType(
            Guid id, 
            string name, 
            string description, 
            IEnumerable<Resource> consumedResources,
            IEnumerable<Item> consumedItems,
            ItemType producedItemType, 
            TimeSpan productionTime,
            IEnumerable<Item> constructionMaterials)
        {
            this.Id = id;
            this.Name = name;
            this.ProducedItemType = producedItemType;
            this.ProductionTime = productionTime;
            this.ConsumedResources = consumedResources.ToList();
            this.ConstructionMaterials = constructionMaterials.ToList();
            this.ConsumedItems = consumedItems.ToList();
            this.Description = description;
        }

        public Guid Id { get; }

        public string Name { get; }

        public string Description { get; }

        public IReadOnlyCollection<Resource> ConsumedResources { get; }

        public IReadOnlyCollection<Item> ConsumedItems { get; }

        public ItemType ProducedItemType { get; }
        
        public TimeSpan ProductionTime { get; }

        public IReadOnlyCollection<Item> ConstructionMaterials { get; }
        
        public override string ToString()
        {
            return this.Name;
        }
    }
}
