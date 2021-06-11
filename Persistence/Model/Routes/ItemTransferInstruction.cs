namespace SpaceLogistic.Persistence.Model.Routes
{
    using System;
    using SpaceLogistic.Core.Model.ShipRoutes;

    public sealed class ItemTransferInstructionData
    {
        public ItemTransferInstructionData(Guid id, Guid itemType, int amount, ItemTransferDirection direction)
        {
            this.Id = id;
            this.ItemType = itemType;
            this.Amount = amount;
            this.Direction = direction;
        }

        public Guid Id { get; }

        public Guid ItemType { get; }

        public int Amount { get; }

        public ItemTransferDirection Direction { get; }
    }
}
