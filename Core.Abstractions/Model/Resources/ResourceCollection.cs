namespace SpaceLogistic.Core.Model.Resources
{
    using System.Collections.Generic;
    using System.Linq;

    public sealed class ResourceCollection : InventoryBase<Resource, ResourceType>
    {
        public ResourceCollection()
            : this(Enumerable.Empty<Resource>())
        {
        }
        
        public ResourceCollection(IEnumerable<Resource> resources)
            : base(resources)
        {
        }

        protected override Resource Create(ResourceType itemType, int amount)
        {
            return new Resource(itemType, amount);
        }
    }
}
