namespace SpaceLogistic.Core.Model
{
    using System.Collections.Generic;
    using System.Linq;

    using SpaceLogistic.Core.Model.Items;

    public sealed class Storage
    {
        private readonly Dictionary<ItemType, Item> items = new Dictionary<ItemType, Item>();

        public IReadOnlyList<Item> Items => this.items.Values.OrderBy(i => i.Amount).ToList();

        public int Get(ItemType itemType)
        {
            if (!this.items.TryGetValue(itemType, out var item))
            {
                return 0;
            }

            return item.Amount;
        }

        public void Add(int amount, ItemType itemType)
        {
            if (amount == 0)
            {
                return;
            }

            this.Add(new Item(itemType, amount));
        }

        public void Add(Item item)
        {
            var amount = item.Amount;
            
            if (this.items.TryGetValue(item.ItemType, out var existingItem))
            {
                amount += existingItem.Amount;
            }

            this.items[item.ItemType] = new Item(item.ItemType, amount);
        }

        public int TakeAll(ItemType itemType)
        {
            if (!this.items.TryGetValue(itemType, out var item))
            {
                return 0;
            }

            this.items.Remove(itemType);

            return item.Amount;
        }

        public int TakeMax(int maxAmount, ItemType itemType)
        {
            if (!this.items.TryGetValue(itemType, out var item))
            {
                return 0;
            }

            if (item.Amount <= maxAmount)
            {
                this.items.Remove(itemType);
                return item.Amount;
            }

            this.items[itemType] = new Item(itemType, item.Amount - maxAmount);

            return maxAmount;
        }
    }
}
