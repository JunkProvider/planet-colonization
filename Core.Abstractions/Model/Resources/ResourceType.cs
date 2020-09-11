namespace SpaceLogistic.Core.Model.Resources
{
    using System;

    public sealed class ResourceType : IComparable, IComparable<ResourceType>
    {
        public ResourceType(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        public string Name { get; }

        public string Description { get; }

        public int CompareTo(object obj)
        {
            if (obj is ResourceType resourceType)
            {
                return this.CompareTo(resourceType);
            }

            return 0;
        }

        public int CompareTo(ResourceType other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }

            return string.Compare(this.Name, other.Name, StringComparison.Ordinal);
        }
    }
}
