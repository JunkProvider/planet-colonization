namespace SpaceLogistic.Core.Commands
{
    using System;
    using System.Linq;

    using SpaceLogistic.Core.CommandPattern;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Core.Model.ShipRoutes;

    public sealed class AddStopCommandHandler : CommandHandlerBase<AddRouteStopCommand>
    {
        private readonly Game game;

        public AddStopCommandHandler(Game game)
        {
            this.game = game;
        }

        public override bool CanExecute(AddRouteStopCommand command)
        {
            if (!this.TryGetEntities(command, out var route, out var orbitalLocation))
            {
                return false;
            }

            if (route.Stops.LastOrDefault()?.Location == orbitalLocation)
            {
                return false;
            }

            return true;
        }

        public override void Execute(AddRouteStopCommand command)
        {
            if (!this.CanExecute(command))
            {
                throw new InvalidOperationException("Can not execute command.");
            }

            if (!this.TryGetEntities(command, out var route, out var orbitalLocation))
            {
                throw new InvalidOperationException("Can not execute command.");
            }

            route.AddStop(new RouteStop(orbitalLocation, RefuelBehavior.NoRefuel));
        }
        
        private bool TryGetEntities(AddRouteStopCommand command, out Route route, out OrbitalLocation orbitalLocation)
        {
            route = this.game.Routes.FirstOrDefault(r => r.Id == command.RouteId);

            if (route == null)
            {
                orbitalLocation = default(OrbitalLocation);
                return false;
            }

            orbitalLocation = this.game.CelestialSystem
                .GetOrbitalLocations()
                .FirstOrDefault(l => l.Id == command.OrbitalLocationId);

            if (orbitalLocation == null)
            {
                return false;
            }

            return true;
        }
    }
}