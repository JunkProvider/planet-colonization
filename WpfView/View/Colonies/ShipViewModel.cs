namespace SpaceLogistic.WpfView.ViewModel.Colonies
{
    using System;

    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.Ships;

    public sealed class ShipViewModel : ViewModelBase, IIdentity
    {
        private Guid id;

        private string name;

        public Guid Id
        {
            get => id;
            private set => SetProperty(ref id, value);
        }

        public string Name
        {
            get => name;
            private set => SetProperty(ref name, value);
        }

        public void Update(Ship ship)
        {
            this.Id = ship.Id;
            this.Name = ship.Name;
        }
    }
}
