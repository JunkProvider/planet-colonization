namespace SpaceLogistic.WpfView.ViewModel.Colonies
{
    using System;

    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Application.Commands;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.Ships;

    public sealed class ShipViewModel : ViewModelBase, IIdentity
    {
        private readonly ICommandDispatcher commandDispatcher;

        private Guid id;

        private string name;

        public ShipViewModel(ICommandDispatcher commandDispatcher)
        {
            this.commandDispatcher = commandDispatcher;
        }
        
        public Guid Id
        {
            get => id;
            private set => SetProperty(ref id, value);
        }

        public string Name
        {
            get => name;
            set => this.commandDispatcher.Execute(new RenameShipCommand(this.id, value));
        }

        public void Update(Ship ship)
        {
            this.Id = ship.Id;
            this.SetProperty(ref this.name, ship.Name, nameof(this.Name));
        }
    }
}
