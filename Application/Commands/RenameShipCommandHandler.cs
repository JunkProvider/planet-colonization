namespace SpaceLogistic.Application.Commands
{
    using System;

    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.Ships;

    public sealed class RenameShipCommandHandler : CommandHandlerBase<RenameShipCommand>
    {
        private readonly IGameProvider gameProvider;

        public RenameShipCommandHandler(IGameProvider gameProvider)
        {
            this.gameProvider = gameProvider;
        }

        public override bool CanExecute(RenameShipCommand command)
        {
            if (!this.TryGetEntities(command, out _))
            {
                return false;
            }

            return true;
        }

        public override void Execute(RenameShipCommand command)
        {
            if (!this.TryGetEntities(command, out var ship))
            {
                throw new InvalidOperationException("Can not execute command.");
            }

            ship.Name = command.NewName;
        }

        private bool TryGetEntities(RenameShipCommand command, out Ship ship)
        {
            var game = this.gameProvider.Get();
            return game.TryGetShip(command.ShipId, out ship);
        }
    }
}