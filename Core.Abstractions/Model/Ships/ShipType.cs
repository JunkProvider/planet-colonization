namespace SpaceLogistic.Core.Model.Ships
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SpaceLogistic.Core.Model.Items;

    public sealed class ShipType : IIdentity
    {
        public ShipType(Guid id, string name, string description, double emptyMass, double fuelCapacity, IEnumerable<Item> constructionMaterials, TimeSpan constructionTime)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.EmptyMass = emptyMass;
            this.FuelCapacity = fuelCapacity;
            this.ConstructionMaterials = constructionMaterials.ToList();
            this.ConstructionTime = constructionTime;
        }

        public Guid Id { get; }

        public string Name { get; }

        public string Description { get; }

        public double EmptyMass { get; }

        public double FuelCapacity { get; }

        public IReadOnlyCollection<Item> ConstructionMaterials { get; }

        public TimeSpan ConstructionTime { get; }
    }
}
