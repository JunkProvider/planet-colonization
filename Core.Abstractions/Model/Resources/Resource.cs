namespace SpaceLogistic.Core.Model.Resources
{
    public sealed class Resource : IInventoryItem<ResourceType>
    {
        public Resource(ResourceType resourceType, int amount)
        {
            this.ResourceType = resourceType;
            this.Amount = amount;
        }

        public ResourceType ResourceType { get; }

        public string Name => this.ResourceType.Name;

        ResourceType IInventoryItem<ResourceType>.ItemType => this.ResourceType;
        
        public int Amount { get; }
    }
}
