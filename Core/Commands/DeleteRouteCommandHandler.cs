namespace SpaceLogistic.Core.Commands
{
    using System;
    using System.Linq;

    using SpaceLogistic.Core.CommandPattern;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.ShipRoutes;

    public sealed class DeleteRouteCommandHandler : CommandHandlerBase<DeleteRouteCommand>
    {
        private readonly Game game;

        public DeleteRouteCommandHandler(Game game)
        {
            this.game = game;
        }

        public override bool CanExecute(DeleteRouteCommand command)
        {
            if (!this.TryGetEntities(command, out _))
            {
                return false;
            }

            return true;
        }

        public override void Execute(DeleteRouteCommand command)
        {
            if (!this.TryGetEntities(command, out var route))
            {
                throw new InvalidOperationException("Can not execute command.");
            }

            foreach (var ship in this.game.Ships.Where(s => s.Route == route))
            {
                ship.Route = null;
            }

            this.game.DeleteRoute(route.Id);
        }

        private bool TryGetEntities(DeleteRouteCommand command, out Route route)
        {
            route = this.game.Routes.FirstOrDefault(r => r.Id == command.RouteId);

            if (route == null)
            {
                return false;
            }

            return true;
        }
    }
}