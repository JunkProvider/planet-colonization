namespace SpaceLogistic.Application.Commands
{
    using System;
    using System.Linq;

    using SpaceLogistic.Application.CommandPattern;
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

            if (!this.TryGetEntities(command, out var route, out var location))
            {
                throw new InvalidOperationException("Can not execute command.");
            }

            route.AddStop(new RouteStop(location, RefuelBehavior.NoRefuel));
        }
        
        private bool TryGetEntities(AddRouteStopCommand command, out Route route, out ILocation location)
        {
            route = this.game.Routes.FirstOrDefault(r => r.Id == command.RouteId);

            if (route == null)
            {
                location = default(OrbitalLocation);
                return false;
            }

            if (!this.game.CelestialSystem.TryGetLocation(command.LocationId, out location))
            {
                return false;
            }

            if (location == null)
            {
                return false;
            }

            return true;
        }
    }
}