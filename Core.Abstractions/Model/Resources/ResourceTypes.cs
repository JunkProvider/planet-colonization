namespace SpaceLogistic.Core.Model.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class ResourceTypes
    {
        public ResourceType Water { get; } = new ResourceType(
            Guid.Parse("40613b5b-d3b8-471f-a271-ed0234c85ef0"), 
            "Water",
            "Used to produce fuel and build farms.");

        public ResourceType Deuterium { get; } = new ResourceType(
            Guid.Parse("40613b5b-d3b8-471f-a271-ed0234c85ef1"),
            "Deuterium",
            "Used to generate energy.");

        public ResourceType Iron { get; } = new ResourceType(
            Guid.Parse("40613b5b-d3b8-471f-a271-ed0234c85ef2"),
            "Iron",
            "Used to build structures.");

        public ResourceType Carbon { get; } = new ResourceType(
            Guid.Parse("40613b5b-d3b8-471f-a271-ed0234c85ef3"),
            "Carbon",
            "Used to build ships.");

        private IReadOnlyCollection<ResourceType> AllResourceTypes { get; }

        private IReadOnlyDictionary<string, ResourceType> AllResourceTypesByNames { get; }

        public ResourceTypes()
        {
            this.AllResourceTypes = new List<ResourceType>
                {
                    this.Water,
                    this.Deuterium,
                    this.Iron,
                    this.Carbon
                };

            this.AllResourceTypesByNames = this.AllResourceTypes.ToDictionary(r => r.Name);
        }

        public ResourceType GetByName(string resourceName)
        {
            return this.AllResourceTypesByNames[resourceName];
        }

        public ResourceType GetById(Guid id)
        {
            return this.AllResourceTypes.First(r => r.Id == id);
        }
    }
}