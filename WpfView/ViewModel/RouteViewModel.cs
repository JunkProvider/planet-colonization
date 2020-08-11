namespace SpaceLogistic.WpfView.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using SpaceLogistic.Core.CommandPattern;
    using SpaceLogistic.Core.Commands;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.ShipRoutes;
    using SpaceLogistic.WpfView.Utility;

    public sealed class RouteViewModel : ViewModelBase, IIdentity
    {
        private readonly ICommandDispatcher commandDispatcher;

        private readonly Game game;

        private readonly Route route;

        private ObservableCollection<RouteStopViewModel> stops = new ObservableCollection<RouteStopViewModel>();

        private ObservableCollection<ShipViewModel> assignableShips = new ObservableCollection<ShipViewModel>();

        private ObservableCollection<ShipViewModel> assignedShips = new ObservableCollection<ShipViewModel>();

        private string name;

        private ObservableCollection<OrbitalLocationViewModel> availableStops = new ObservableCollection<OrbitalLocationViewModel>();

        private OrbitalLocationViewModel selectedAddStopOption;

        public RouteViewModel(Game game, Route route, ICommandDispatcher commandDispatcher)
        {
            this.game = game;
            this.route = route;
            this.commandDispatcher = commandDispatcher;

            this.DeleteCommand = new DelegateCommand(this.Delete);
            this.AddStopCommand = new DelegateCommand<OrbitalLocationViewModel>(this.AddStop);
            this.AssignShipCommand = new DelegateCommand<ShipViewModel>(this.AssignShip);

            this.Update();
        }

        public Guid Id => this.route.Id;

        public string Name
        {
            get => this.name;
            set => this.name = value;
        }

        public ObservableCollection<OrbitalLocationViewModel> AvailableStops
        {
            get => this.availableStops;
            private set => this.SetProperty(ref this.availableStops, value);
        }

        public OrbitalLocationViewModel SelectedAddStopOption
        {
            get => this.selectedAddStopOption;
            set => this.SetProperty(ref this.selectedAddStopOption, value);
        }

        public ObservableCollection<RouteStopViewModel> Stops
        {
            get => this.stops;
            set => this.SetProperty(ref this.stops, value);
        }

        public ObservableCollection<ShipViewModel> AssignableShips
        {
            get => this.assignableShips;
            private set => this.SetProperty(ref this.assignableShips, value);
        }

        public ObservableCollection<ShipViewModel> AssignedShips
        {
            get => this.assignedShips;
            private set => SetProperty(ref this.assignedShips, value);
        }

        public ICommand DeleteCommand { get; }

        public ICommand AddStopCommand { get; }

        public ICommand AssignShipCommand { get; }

        public void Update()
        {
            this.Name = this.route.Name;

            this.AvailableStops = ViewModelHelper.Update(
                this.AvailableStops,
                this.game.CelestialSystem.GetOrbitalLocations().Except(this.route.Stops.Select(s => s.Location)),
                location => new OrbitalLocationViewModel(location), 
                (location, locationViewModel) => locationViewModel.Update());

            this.Stops = ViewModelHelper.Update(
                this.Stops,
                this.route.Stops,
                this.CreateStopViewModel,
                (stopModel, stop) => stop.Update());

            this.AssignableShips = ViewModelHelper.Update(
                this.AssignableShips,
                this.game.Ships.Where(s => s.Route == null),
                this.CreateShipViewModel,
                (shipModel, ship) => ship.Update());

            this.AssignedShips = ViewModelHelper.Update(
                this.assignedShips,
                this.game.Ships.Where(s => s.Route == this.route),
                this.CreateShipViewModel,
                (shipModel, ship) => ship.Update());
        }

        private void Delete()
        {
            this.commandDispatcher.Execute(new DeleteRouteCommand(this.Id));
        }

        private void AddStop(OrbitalLocationViewModel orbitalLocationViewModel)
        {
            this.commandDispatcher.Execute(new AddRouteStopCommand(this.Id, orbitalLocationViewModel.Id));
        }

        private void DeleteStop(Guid stopId)
        {
            this.commandDispatcher.Execute(new RemoveRouteStopCommand(this.route.Id, stopId));
        }
        
        private void AssignShip(ShipViewModel shipViewModel)
        {
            this.commandDispatcher.Execute(new AssignShipCommand(this.Id, shipViewModel.Id));
        }

        private RouteStopViewModel CreateStopViewModel(RouteStop stop)
        {
            return new RouteStopViewModel(
                this.game.ItemTypes,
                stop, 
                this.DeleteStop);
        }

        private ShipViewModel CreateShipViewModel(Ship ship)
        {
            return new ShipViewModel(ship, this.commandDispatcher);
        }
    }
}
