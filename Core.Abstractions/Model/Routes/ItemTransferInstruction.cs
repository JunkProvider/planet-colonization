namespace SpaceLogistic.Core.Model.ShipRoutes
{
    using System;

    public abstract class ItemTransferInstruction : IIdentity
    {
        public ItemTransferInstruction(ItemType itemType, int amount)
        {
            this.ItemType = itemType;
            this.Amount = amount;
        }

        public Guid Id { get; } = Guid.NewGuid();

        public ItemType ItemType { get; set; }

        public int Amount { get; set; }

        public abstract ItemTransferDirection Direction { get; }

        public bool IsLoad => this.Direction == ItemTransferDirection.Load;

        public bool IsUnload => this.Direction == ItemTransferDirection.Unload;
    }
}
