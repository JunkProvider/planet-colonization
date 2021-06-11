namespace SpaceLogistic.Core.Model.Items
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class ItemTypes
    {
        public ItemType Fuel { get; } = new ItemType(
            Guid.Parse("719026b4-cbc1-4706-8dc8-6bc26fd5c1f0"), 
            "Fuel");

        public ItemType Deuterium { get; } = new ItemType(
            Guid.Parse("719026b4-cbc1-4706-8dc8-6bc26fd5c1f1"),
            "Deuterium");

        public ItemType IronOre { get; } = new ItemType(
            Guid.Parse("719026b4-cbc1-4706-8dc8-6bc26fd5c1f2"),
            "Iron Ore");

        public ItemType Steel { get; } = new ItemType(
            Guid.Parse("719026b4-cbc1-4706-8dc8-6bc26fd5c1f3"),
            "Steel");

        public IReadOnlyCollection<ItemType> GetAll()
        {
            return new[] { this.Fuel, this.Deuterium, this.Steel, this.IronOre };
        }

        public ItemType Get(Guid id)
        {
            return this.GetAll().First(i => i.Id == id);
        }
    }
}
