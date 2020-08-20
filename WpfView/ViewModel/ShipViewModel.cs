namespace SpaceLogistic.WpfView.ViewModel
{
    using System;
    using System.Windows.Input;

    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Application.Commands;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.WpfView.Utility;

    public sealed class ShipViewModel : ViewModelBase, IIdentity
    {
        private readonly ICommandDispatcher commandDispatcher;

        private readonly Ship ship;

        private string fuel;

        private string state;

        public ShipViewModel(Ship ship, ICommandDispatcher commandDispatcher)
        {
            this.ship = ship;
            this.commandDispatcher = commandDispatcher;
            this.Name = ship.Name;
            this.CargoBay = new StorageViewModel(ship.CargoBay);

            this.UnassignCommand = new DelegateCommand(this.Deassign);

            this.Update();
        }
        
        public Guid Id => this.ship.Id;

        public string Name { get; }

        public StorageViewModel CargoBay { get; }

        public string Fuel
        {
            get => this.fuel;
            private set
            {
                if (value == fuel)
                {
                    return;
                }
                
                this.fuel = value;
                this.OnPropertyChanged();
            }
        }

        public string State
        {
            get => this.state;
            private set
            {
                if (value == this.state)
                {
                    return;
                }

                this.state = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand UnassignCommand { get; }

        public void Update()
        {
            this.Fuel = $"Fuel: {ship.Fuel:0.0}";

            this.UpdateState();

            this.CargoBay.Update();
        }

        private void UpdateState()
        {
            if (this.ship.Location != null && ship.RefuelingProcess != null)
            {
                this.State = $"{this.ship.Location.Name}: Refueling ({(this.ship.RefuelingProcess.Progress * 100):0}%)";
                return;
            }

            if (this.ship.Location != null)
            {
                this.State = this.ship.Location.Name;
                return;
            }

            if (this.ship.Transfer != null)
            {
                this.State = $"{this.ship.Transfer.Origin.Name} -> {this.ship.Transfer.Destination.Name} ({(this.ship.Transfer.Progress * 100):0}%)";
                return;
            }
        }

        private void Deassign()
        {
            this.commandDispatcher.Execute(new DeassignShipCommand(this.ship.Id));
        }
    }
}
