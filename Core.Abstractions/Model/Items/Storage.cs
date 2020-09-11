namespace SpaceLogistic.Core.Model.Items
{
    using System.Collections.Generic;
    using System.Linq;

    public sealed class Storage : InventoryBase<Item, ItemType>
    {
        public Storage()
            : this(Enumerable.Empty<Item>())
        {
        }

        public Storage(IEnumerable<Item> initialItems)
            : base(initialItems)
        {
        }

        protected override Item Create(ItemType itemType, int amount)
        {
            return new Item(itemType, amount);
        }
    }
}
