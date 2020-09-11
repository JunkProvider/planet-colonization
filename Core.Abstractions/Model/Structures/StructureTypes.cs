namespace SpaceLogistic.Core.Model.Structures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SpaceLogistic.Core.Model.Items;
    using SpaceLogistic.Core.Model.Resources;

    public sealed class StructureTypes
    {
        private readonly IReadOnlyCollection<StructureType> all;

        public StructureType FuelPlant { get; }

        public StructureType DeuteriumFarm { get; }

        public StructureType IronMine { get; }

        public StructureType SteelPlant { get; }

        public StructureTypes(ItemTypes itemTypes, ResourceTypes resourceTypes)
        {
            this.FuelPlant = new StructureType(
                Guid.Parse("b84e313f-7056-4ba6-8d1e-f2c96b37680f"), 
                "Fuel Plant", 
                "Converts water into fuel.",
                new[] { new Resource(resourceTypes.Water, 1) },
                itemTypes.Fuel, 
                TimeSpan.FromSeconds(1),
                new[] { new Item(itemTypes.Steel, 3) });
            
            this.DeuteriumFarm = new StructureType(
                Guid.Parse("024c06a5-45be-4c7b-a7ba-762934c774c7"),
                "Deuterium Farm",
                "Extracts deuterium from water.",
                new[] { new Resource(resourceTypes.Deuterium, 1) },
                itemTypes.Deuterium, 
                TimeSpan.FromSeconds(5),
                new[] { new Item(itemTypes.Steel, 3) });

            this.IronMine = new StructureType(
                Guid.Parse("024c06a5-45be-4c7b-a7ba-762934c774c8"),
                "Iron Mine",
                "Mines raw iron ore from a planet.",
                new[] { new Resource(resourceTypes.Iron, 1) },
                itemTypes.IronOre,
                TimeSpan.FromSeconds(2),
                new[] { new Item(itemTypes.Steel, 3) });

            this.SteelPlant = new StructureType(
                Guid.Parse("024c06a5-45be-4c7b-a7ba-762934c774c9"),
                "Steel Plant",
                "Converts raw iron ore into steel.",
                new[] { new Item(itemTypes.IronOre, 2) },
                itemTypes.Steel,
                TimeSpan.FromSeconds(2),
                new[] { new Item(itemTypes.Steel, 3) });

            this.all = new List<StructureType>
               {
                   this.FuelPlant,
                   this.DeuteriumFarm,
                   this.IronMine,
                   this.SteelPlant
               };
        }

        public IReadOnlyCollection<StructureType> GetAll()
        {
            return this.all;
        }

        public bool TryGet(Guid structureTypeId, out StructureType structureType)
        {
            structureType = this.GetAll().FirstOrDefault(s => s.Id == structureTypeId);
            return structureType != null;
        }
    }
}
