namespace SpaceLogistic.Application.Commands
{
    using System;
    using System.Linq;

    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.ShipRoutes;
    using SpaceLogistic.Core.Model.Ships;

    public sealed class AssignShipCommandHandler : CommandHandlerBase<AssignShipCommand>
    {
        private readonly Game game;

        public AssignShipCommandHandler(Game game)
        {
            this.game = game;
        }

        public override bool CanExecute(AssignShipCommand command)
        {
            if (!this.TryGetEntities(command, out _, out var ship))
            {
                return false;
            }

            return ship.Route == null;
        }

        public override void Execute(AssignShipCommand command)
        {
            if (!this.TryGetEntities(command, out var route, out var ship))
            {
                throw new InvalidOperationException("Can not execute command.");
            }

            if (ship.Route != null)
            {
                throw new InvalidOperationException("Can not assign ship. Ship is already assigned.");
            }

            ship.Route = route;
        }

        private bool TryGetEntities(AssignShipCommand command, out Route route, out Ship ship)
        {
            route = this.game.Routes.FirstOrDefault(r => r.Id == command.RouteId);
            ship = this.game.Ships.FirstOrDefault(s => s.Id == command.ShipId);
            return route != null && ship != null;
        }
    }
}