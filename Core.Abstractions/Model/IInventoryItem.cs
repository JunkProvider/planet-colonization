namespace SpaceLogistic.Core.Model
{
    public interface IInventoryItem<out TItemType>
    {
        TItemType ItemType { get; }

        int Amount { get; }
    }
}