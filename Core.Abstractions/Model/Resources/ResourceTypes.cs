namespace SpaceLogistic.Core.Model.Resources
{
    using System.Collections.Generic;
    using System.Linq;

    public sealed class ResourceTypes
    {
        public ResourceType Water { get; } = new ResourceType(
            "Water",
            "Used to produce fuel and build farms.");

        public ResourceType Deuterium { get; } = new ResourceType(
            "Deuterium",
            "Used to generate energy.");

        public ResourceType Iron { get; } = new ResourceType(
            "Iron",
            "Used to build structures.");

        public ResourceType Carbon { get; } = new ResourceType(
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
    }
}