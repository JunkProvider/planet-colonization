namespace SpaceLogistic.Application.Commands
{
    using System;
    using System.Linq;

    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.ShipRoutes;

    public sealed class RemoveStopCommandHandler : CommandHandlerBase<RemoveRouteStopCommand>
    {
        private readonly IGameProvider gameProvider;

        public RemoveStopCommandHandler(IGameProvider gameProvider)
        {
            this.gameProvider = gameProvider;
        }

        public override bool CanExecute(RemoveRouteStopCommand command)
        {
            if (!this.TryGetEntities(command, out _))
            {
                return false;
            }
            
            return true;
        }

        public override void Execute(RemoveRouteStopCommand command)
        {
            if (!this.TryGetEntities(command, out var route))
            {
                throw new InvalidOperationException("Can not execute command.");
            }

            route.RemoveStop(command.StopId);
        }

        private bool TryGetEntities(RemoveRouteStopCommand command, out Route route)
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