namespace SpaceLogistic.Core.Model.Items
{
    public sealed class Item
    {
        public Item(ItemType itemType, int amount)
        {
            this.ItemType = itemType;
            this.Amount = amount;
        }

        public ItemType ItemType { get; }

        public string Name => this.ItemType.Name;

        public int Amount { get; }

        public override string ToString()
        {
            return $"{this.Amount} {this.Name}";
        }
    }
}