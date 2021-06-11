namespace SpaceLogistic.Application.Commands
{
    using System;
    using System.Linq;

    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.Ships;

    public sealed class DeassignShipCommandHandler : CommandHandlerBase<DeassignShipCommand>
    {
        private readonly IGameProvider gameProvider;

        public DeassignShipCommandHandler(IGameProvider gameProvider)
        {
            this.gameProvider = gameProvider;
        }

        public override bool CanExecute(DeassignShipCommand command)
        {
            if (!this.TryGetEntities(command, out var ship))
            {
                return false;
            }

            return ship.Route != null;
        }

        public override void Execute(DeassignShipCommand command)
        {
            if (!this.TryGetEntities(command, out var ship))
            {
                throw new InvalidOperationException("Can not execute command.");
            }

            ship.Route = null;
        }

        private bool TryGetEntities(DeassignShipCommand command, out Ship ship)
        {
            var game = this.gameProvider.Get();
            ship = game.Ships.FirstOrDefault(s => s.Id == command.ShipId);
            return ship != null;
        }
    }
}