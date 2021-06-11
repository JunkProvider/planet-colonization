namespace SpaceLogistic.WpfView.View.Routes
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Application.Commands;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.ShipRoutes;
    using SpaceLogistic.WpfView.Utility;

    public sealed class RoutePageViewModel : ViewModelBase, IPageViewModel
    {
        private readonly ICommandDispatcher commandDispatcher;

        private readonly Game game;
        
        private ObservableCollection<RouteViewModel> routes = new ObservableCollection<RouteViewModel>();

        private RouteViewModel selectedRoute;
        
        public RoutePageViewModel(ICommandDispatcher commandDispatcher, Game game)
        {
            this.commandDispatcher = commandDispatcher;
            this.game = game;
            this.CelestialSystem = new CelestialSystemViewModel(
                game.CelestialSystem, 
                new DelegateCommand<StationViewModel>(this.SelectStation, this.CanSelectStation),
                new DelegateCommand<CelestialBodyViewModel>(this.SelectBody, this.CanSelectBody));

            this.AddRouteCommand = new DelegateCommand(this.AddRoute);

            this.Update(game);
        }

        public string Title => "Routes";

        public CelestialSystemViewModel CelestialSystem { get; }

        public ObservableCollection<RouteViewModel> Routes
        {
            get => this.routes;
            set => this.SetProperty(ref this.routes, value);
        }

        public RouteViewModel SelectedRoute
        {
            get => this.selectedRoute;
            set => this.SetProperty(ref this.selectedRoute, value);
        }

        public ICommand AddRouteCommand { get; }

        public void Update(Game game)
        {
            this.CelestialSystem.Update();

            this.Routes = ViewModelHelper.UpdateCollectionByIdentity(
                this.Routes,
                // TODO: Add routes property to game
                this.game.Routes,
                this.CreateRouteViewModel, 
                (routeModel, route) => route.Update());
        }

        private void AddRoute()
        {
            var route = new Route("Unnamed Route", Enumerable.Empty<RouteStop>());
            this.game.AddRoute(route);
            var routeViewModel = this.CreateRouteViewModel(route);
            this.Routes.Add(routeViewModel);
            this.SelectedRoute = routeViewModel;
        }

        private bool CanSelectStation(StationViewModel station)
        {
            var locationId = this.GetLocationId(station);

            var command = new AddRouteStopCommand(this.selectedRoute?.Id ?? Guid.Empty, locationId);
            
            return this.commandDispatcher.CanExecute(command);
        }

        private void SelectStation(StationViewModel station)
        {
            var locationId = this.GetLocationId(station);

            var command = new AddRouteStopCommand(this.selectedRoute?.Id ?? Guid.Empty, locationId);
            
            this.commandDispatcher.Execute(command);
        }

        private bool CanSelectBody(CelestialBodyViewModel location)
        {
            var command = new AddRouteStopCommand(this.selectedRoute?.Id ?? Guid.Empty, location.Id);
            return this.commandDispatcher.CanExecute(command);
        }

        private void SelectBody(CelestialBodyViewModel location)
        {
            var command = new AddRouteStopCommand(this.selectedRoute?.Id ?? Guid.Empty, location.Id);
            this.commandDispatcher.Execute(command);
        }

        private RouteViewModel CreateRouteViewModel(Route route)
        {
            return new RouteViewModel(this.game, route, this.commandDispatcher);
        }

        private Guid GetLocationId(StationViewModel station)
        {
            return this.game.CelestialSystem.GetOrbitalLocations()
                .FirstOrDefault(l => l.Colony?.Id == station.Id)
                ?.Id ?? Guid.Empty;
        }
    }
}