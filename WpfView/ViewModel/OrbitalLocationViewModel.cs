namespace SpaceLogistic.WpfView.ViewModel
{
    using System;

    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.Celestials;

    public sealed class OrbitalLocationViewModel : ViewModelBase, IIdentity
    {
        private readonly OrbitalLocation location;

        private string name;

        private string path;

        public OrbitalLocationViewModel(OrbitalLocation location)
        {
            this.location = location;
        }

        public Guid Id => this.location.Id;

        public string Name
        {
            get => this.name;
            private set => this.SetProperty(ref this.name, value);
        }

        public string Path
        {
            get => this.path;
            private set => this.SetProperty(ref this.path, value);
        }

        public void Update()
        {
            var name = this.location.Name;

            if (this.location.Object != null)
            {
                name += " - " + this.location.Object.Name;
            }

            this.Name = name;
            
            var parentPath = this.location.System.GetCentralBodyPathTillRoot();
            
            this.Path = string.IsNullOrEmpty(parentPath)
                ? name
                : $"{parentPath} > {name}";
        }
    }
}
