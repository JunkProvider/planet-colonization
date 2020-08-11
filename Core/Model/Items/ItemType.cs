namespace SpaceLogistic.Core.Model
{
    public sealed class ItemType
    {
        public ItemType(string name)
        {
            this.Name = name;
        }

        public string Name { get; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}