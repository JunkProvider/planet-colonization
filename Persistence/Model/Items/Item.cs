namespace SpaceLogistic.Persistence.Model.Items
{
    using System;

    public sealed class ItemData
    {
        public ItemData(Guid itemType, int amount)
        {
            this.ItemType = itemType;
            this.Amount = amount;
        }

        public Guid ItemType { get; }

        public int Amount { get; }

        public override string ToString()
        {
            return $"{this.Amount} {this.ItemType}";
        }
    }
}