namespace SpaceLogistic.WpfView.ViewModel
{
    using System;

    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.Celestials;

    public sealed class StopOptionViewModel : ViewModelBase, IIdentity
    {
        private readonly ILocation location;

        private string name;

        public StopOptionViewModel(ILocation location)
        {
            this.location = location;
        }

        public Guid Id => this.location.Id;

        public string Name
        {
            get => this.name;
            private set => this.SetProperty(ref this.name, value);
        }

        public void Update()
        {
            var displayName = this.location.FullName;

            if (this.location.Colony != null)
            {
                displayName += " - " + this.location.Colony.Name;
            }

            this.Name = displayName;
        }
    }
}
