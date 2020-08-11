namespace SpaceLogistic.Core.Model.Resources
{
    public sealed class ResourceType
    {
        public ResourceType(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }
        
        public string Name { get; }

        public string Description { get; }
    }
}
