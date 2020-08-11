namespace SpaceLogistic.Core.Model.Resources
{
    public sealed class Resource
    {
        public Resource(ResourceType resourceType, double amount)
        {
            this.ResourceType = resourceType;
            this.Amount = amount;
        }

        public ResourceType ResourceType { get; }

        public double Amount { get; }
    }
}
