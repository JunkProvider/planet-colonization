namespace SpaceLogistic.Persistence.Model.Resources
{
    using System.Collections.Generic;

    public sealed class ResourceCollectionData
    {
        public ResourceCollectionData(List<ResourceData> items)
        {
            this.Items = items;
        }
        
        public List<ResourceData> Items { get; }
    }
}
