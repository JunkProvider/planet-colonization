namespace SpaceLogistic.Core.Model.Ships
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SpaceLogistic.Core.Model.Items;

    public sealed class ShipTypes
    {
        public ShipType SmallCargoShip { get; }

        public ShipType LargeCargoShip { get; }

        public ShipTypes(ItemTypes itemTypes)
        {
            this.SmallCargoShip = new ShipType(
                Guid.Parse("da1f5268-dd6e-4e02-826d-719c2687bf32"),
                "Small Cargo Ship",
                "A small cargo ship.",
                10e3,
                15,
                new[] { new Item(itemTypes.Steel, 5) },
                TimeSpan.FromSeconds(5));

            this.LargeCargoShip = new ShipType(
                Guid.Parse("da1f5268-dd6e-4e02-826d-719c2687bf33"),
                "Large Cargo Ship",
                "A large cargo ship.",
                25e3,
                40,
                new[] { new Item(itemTypes.Steel, 10) },
                TimeSpan.FromSeconds(15));
        }

        public IReadOnlyCollection<ShipType> GetAll()
        {
            return new[] { this.SmallCargoShip, this.LargeCargoShip };
        }
        
        public bool TryGet(Guid id, out ShipType shipType)
        {
            shipType = this.GetAll().FirstOrDefault(t => t.Id == id);
            return shipType != null;
        }
    }
}