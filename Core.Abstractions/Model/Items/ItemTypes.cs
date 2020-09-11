namespace SpaceLogistic.Core.Model.Items
{
    using System.Collections.Generic;

    public sealed class ItemTypes
    {
        public ItemType Fuel { get; } = new ItemType("Fuel");

        public ItemType Deuterium { get; } = new ItemType("Deuterium");

        public ItemType IronOre { get; } = new ItemType("Iron Ore");

        public ItemType Steel { get; } = new ItemType("Steel");

        public IReadOnlyCollection<ItemType> GetAll()
        {
            return new[] { this.Fuel, this.Deuterium, this.Steel };
        }
    }
}
