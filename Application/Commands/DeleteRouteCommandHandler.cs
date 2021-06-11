namespace SpaceLogistic.Application.Commands
{
    using System;
    using System.Linq;

    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.ShipRoutes;

    public sealed class DeleteRouteCommandHandler : CommandHandlerBase<DeleteRouteCommand>
    {
        private readonly IGameProvider gameProvider;

        public DeleteRouteCommandHandler(IGameProvider gameProvider)
        {
            this.gameProvider = gameProvider;
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
            
            var game = this.gameProvider.Get();

            foreach (var ship in game.Ships.Where(s => s.Route == route))
            {
                ship.Route = null;
            }

            game.DeleteRoute(route.Id);
        }

        private bool TryGetEntities(DeleteRouteCommand command, out Route route)
        {
            var game = this.gameProvider.Get();
            
            route = game.Routes.FirstOrDefault(r => r.Id == command.RouteId);

            if (route == null)
            {
                return false;
            }

            return true;
        }
    }
}