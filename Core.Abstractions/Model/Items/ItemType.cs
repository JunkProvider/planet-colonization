namespace SpaceLogistic.Core.Model
{
    using System;

    public sealed class ItemType : IIdentity, IComparable, IComparable<ItemType>
    {
        public ItemType(Guid id, string name)
        {
            this.Name = name;
            this.Id = id;
        }

        public Guid Id { get; }
        
        public string Name { get; }

        public override string ToString()
        {
            return this.Name;
        }

        public int CompareTo(object obj)
        {
            if (obj is ItemType itemType)
            {
                return this.CompareTo(itemType);
            }

            return 0;
        }

        public int CompareTo(ItemType other)
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