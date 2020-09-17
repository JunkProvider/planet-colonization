namespace SpaceLogistic.Application.Commands
{
    using System;

    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.Ships;
    using SpaceLogistic.Core.Model.Stations;

    public sealed class AddShipCommandHandler : CommandHandlerBase<AddShipCommand>
    {
        private readonly Game game;

        public AddShipCommandHandler(Game game)
        {
            this.game = game;
        }

        public override bool CanExecute(AddShipCommand command)
        {
            if (!this.TryGetEntities(command, out var colony, out var shipType))
            {
                return false;
            }

            if (!colony.Warehouse.Contains(shipType.ConstructionMaterials))
            {
                return false;
            }

            return true;
        }

        public override void Execute(AddShipCommand command)
        {
            if (!this.TryGetEntities(command, out var colony, out var shipType))
            {
                throw new InvalidOperationException("Can not execute command.");
            }

            if (!colony.Warehouse.TryTake(shipType.ConstructionMaterials))
            {
                throw new InvalidOperationException("Can not execute command.");
            }

            colony.AddShipConstructionProcess(new ShipConstructionProcess(
                shipType.ConstructionTime, shipType, colony));
        }

        private bool TryGetEntities(AddShipCommand command, out Colony colony, out ShipType shipType)
        {
            if (!this.game.TryGetColony(command.ColonyId, out colony))
            {
                shipType = default(ShipType);
                return false;
            }

            if (!this.game.ShipTypes.TryGet(command.ShipTypeId, out shipType))
            {
                return false;
            }

            return true;
        }
    }
}