namespace SpaceLogistic.Persistence.Model.Items
{
    using System.Collections.Generic;

    public sealed class StorageData
    {
        public StorageData(List<ItemData> items)
        {
            this.Items = items;
        }
        
        public List<ItemData> Items { get; set; }
    }
}
