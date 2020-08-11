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
    using SpaceLogistic.Core.Model.Stations;
    using SpaceLogistic.Utility;
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
            this.CelestialSystem = new CelestialSystemViewModel(game.CelestialSystem, new DelegateCommand<StationViewModel>(this.SelectStation, this.CanSelectStation));

            this.AddRouteCommand = new DelegateCommand(this.AddRoute);

            this.Update();
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
            get => selectedRoute;
            set => SetProperty(ref selectedRoute, value);
        }

        public ICommand AddRouteCommand { get; }

        public void Update()
        {
            this.CelestialSystem.Update();

            this.Routes = ViewModelHelper.Update(
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
            var locationId = this.game.CelestialSystem.GetOrbitalLocations()
                .Select(l => l.Object)
                .OfType<Station>()
                .FirstOrDefault(s => s.Id == station.Id)
                ?.Location?.Id ?? Guid.Empty;

            var command = new AddRouteStopCommand(this.selectedRoute?.Id ?? Guid.Empty, locationId);
            
            return this.commandDispatcher.CanExecute(command);
        }

        private void SelectStation(StationViewModel station)
        {
            var locationId = this.game.CelestialSystem.GetOrbitalLocations()
                .Select(l => l.Object)
                .OfType<Station>()
                .FirstOrDefault(s => s.Id == station.Id)
                ?.Location?.Id ?? Guid.Empty;

            var command = new AddRouteStopCommand(this.selectedRoute?.Id ?? Guid.Empty, locationId);
            
            this.commandDispatcher.Execute(command);

            this.selectedRoute?.Update();
        }

        private RouteViewModel CreateRouteViewModel(Route route)
        {
            return new RouteViewModel(this.game, route, this.commandDispatcher);
        }
    }
}