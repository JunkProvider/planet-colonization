namespace SpaceLogistic.Core.Model.ShipRoutes
{
    public sealed class LoadInstruction : ItemTransferInstruction
    {
        public LoadInstruction(ItemType itemType, int amount)
            : base(itemType, amount)
        {
        }

        public override ItemTransferDirection Direction => ItemTransferDirection.Load;
    }
}