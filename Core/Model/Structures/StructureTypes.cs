namespace SpaceLogistic.Core.Model.Structures
{
    using System;

    using SpaceLogistic.Core.Model.Items;

    public sealed class StructureTypes
    {
        public StructureType FuelPlant { get; }

        public StructureType DeuteriumFarm { get; }

        public StructureTypes(ItemTypes itemTypes)
        {
            this.FuelPlant = new StructureType("Fuel Plant", itemTypes.Fuel, TimeSpan.FromSeconds(1));
            this.DeuteriumFarm = new StructureType("Deuterium Farm", itemTypes.Deuterium, TimeSpan.FromSeconds(1));
        }
    }
}
