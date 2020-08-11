namespace SpaceLogistic.Core.Commands
{
    using System;
    using System.Linq;

    using SpaceLogistic.Core.CommandPattern;
    using SpaceLogistic.Core.Model;

    public sealed class DeassignShipCommandHandler : CommandHandlerBase<DeassignShipCommand>
    {
        private readonly Game game;

        public DeassignShipCommandHandler(Game game)
        {
            this.game = game;
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
            ship = this.game.Ships.FirstOrDefault(s => s.Id == command.ShipId);
            return ship != null;
        }
    }
}