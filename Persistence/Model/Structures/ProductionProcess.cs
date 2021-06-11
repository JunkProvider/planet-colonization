namespace SpaceLogistic.Persistence.Model.Structures
{
    using System;

    public sealed class ProductionProcessData : TimedProcessData
    {
        public ProductionProcessData(TimeSpan totalTime, double progress, Guid producedItemType) : base(totalTime, progress)
        {
            this.ProducedItemType = producedItemType;
        }

        public Guid ProducedItemType { get; }
    }
}