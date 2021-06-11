namespace SpaceLogistic.WpfView.View.Colonies
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
            get => this.id;
            private set => this.SetProperty(ref this.id, value);
        }

        public string Name
        {
            get => this.name;
            set => this.commandDispatcher.Execute(new RenameShipCommand(this.id, value));
        }

        public void Update(Ship ship)
        {
            this.Id = ship.Id;
            this.SetProperty(ref this.name, ship.Name, nameof(this.Name));
        }
    }
}
