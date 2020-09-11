namespace SpaceLogistic.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SpaceLogistic.Utility;

    public abstract class InventoryBase<TItem, TItemType>
        where TItem : IInventoryItem<TItemType>
    {
        private readonly Dictionary<TItemType, TItem> items = new Dictionary<TItemType, TItem>();

        protected InventoryBase(IEnumerable<TItem> initialItems)
        {
            this.items = initialItems.ToDictionary(i => i.ItemType);
        }

        public bool IsEmpty => this.items.Count == 0;

        public IReadOnlyCollection<TItem> Items => this.items.Values.OrderBy(i => i.ItemType).ToList();
        
        public bool Contains(IEnumerable<TItem> searchedItems)
        {
            foreach (var searchedItem in searchedItems)
            {
                if (!this.items.TryGetValue(searchedItem.ItemType, out var item) || item.Amount < searchedItem.Amount)
                {
                    return false;
                }
            }

            return true;
        }

        public int Get(TItemType itemType)
        {
            if (!this.items.TryGetValue(itemType, out var item))
            {
                return 0;
            }

            return item.Amount;
        }

        public void Add(int amount, TItemType itemType)
        {
            if (amount == 0)
            {
                return;
            }

            this.Add(this.Create(itemType, amount));
        }

        public void Add(IEnumerable<TItem> itemToAdds)
        {
            foreach (var itemToAdd in itemToAdds)
            {
                this.Add(itemToAdd);
            }
        }

        public void Add(TItem itemToAdd)
        {
            var amount = itemToAdd.Amount;

            if (this.items.TryGetValue(itemToAdd.ItemType, out var item))
            {
                amount += item.Amount;
            }

            this.items[itemToAdd.ItemType] = this.Create(itemToAdd.ItemType, amount);
        }

        public int TakeAll(TItemType itemType)
        {
            if (!this.items.TryGetValue(itemType, out var item))
            {
                return 0;
            }

            this.items.Remove(itemType);

            return item.Amount;
        }

        public int TakeMax(int maxAmount, TItemType itemType)
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

            this.items[itemType] = this.Create(itemType, item.Amount - maxAmount);

            return maxAmount;
        }

        public void Take(IEnumerable<TItem> itemsToTake)
        {
            if (!this.TryTake(itemsToTake))
            {
                throw new InvalidOperationException("Could not take requested items. Not all items were available.");
            }
        }

        public bool TryTake(IEnumerable<TItem> itemsToTake)
        {
            var itemsToReplace = new List<TItem>();
            var itemsToRemove = new List<TItemType>();

            foreach (var itemToTake in itemsToTake)
            {
                if (!this.items.TryGetValue(itemToTake.ItemType, out var item) || item.Amount < itemToTake.Amount)
                {
                    return false;
                }

                var newAmount = item.Amount - itemToTake.Amount;

                if (newAmount > 0)
                {
                    itemsToReplace.Add(this.Create(item.ItemType, newAmount));
                }
                else
                {
                    itemsToRemove.Add(item.ItemType);
                }
            }

            this.items.Remove(itemsToRemove);
            this.items.Replace(itemsToReplace, item => item.ItemType);

            return true;
        }
        
        protected abstract TItem Create(TItemType itemType, int amount);
    }
}
