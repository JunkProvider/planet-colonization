namespace SpaceLogistic.Persistence.Model.Resources
{
    using System;

    public sealed class ResourceData
    {
        public ResourceData(Guid resourceType, int amount)
        {
            this.ResourceType = resourceType;
            this.Amount = amount;
        }

        public Guid ResourceType { get; }
        
        public int Amount { get; }
    }
}
