namespace SpaceLogistic.Core.Model.Structures
{
    using System;

    public sealed class ProductionProcess : TimedProcess
    {
        public ProductionProcess(ItemType producedItemType, TimeSpan totalTime)
            : base(totalTime)
        {
            this.ProducedItemType = producedItemType;
        }

        public ItemType ProducedItemType { get; }
    }
}