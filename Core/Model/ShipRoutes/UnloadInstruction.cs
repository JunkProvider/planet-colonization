namespace SpaceLogistic.Core.Model.ShipRoutes
{
    public sealed class UnloadInstruction : ItemTransferInstruction
    {
        public UnloadInstruction(ItemType itemType, int amount)
            : base(itemType, amount)
        {
        }

        public override ItemTransferDirection Direction => ItemTransferDirection.Unload;
    }
}