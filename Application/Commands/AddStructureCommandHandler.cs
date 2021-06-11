namespace SpaceLogistic.Application.Commands
{
    using System;

    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.Stations;
    using SpaceLogistic.Core.Model.Structures;

    public sealed class AddStructureCommandHandler : CommandHandlerBase<AddStructureCommand>
    {
        private readonly IGameProvider gameProvider;

        public AddStructureCommandHandler(IGameProvider gameProvider)
        {
            this.gameProvider = gameProvider;
        }

        public override bool CanExecute(AddStructureCommand command)
        {
            if (!this.TryGetEntities(command, out var colony, out var structureType))
            {
                return false;
            }

            if (!colony.Warehouse.Contains(structureType.ConstructionMaterials))
            {
                return false;
            }

            return true;
        }

        public override void Execute(AddStructureCommand command)
        {
            if (!this.TryGetEntities(command, out var colony, out var structureType))
            {
                throw new InvalidOperationException("Can not execute command.");
            }

            if (!colony.Warehouse.TryTake(structureType.ConstructionMaterials))
            {
                throw new InvalidOperationException("Can not execute command.");
            }

            colony.AddStructure(new Structure(structureType));
        }

        private bool TryGetEntities(AddStructureCommand command, out Colony colony, out StructureType structureType)
        {
            var game = this.gameProvider.Get();
            
            if (!game.TryGetColony(command.ColonyId, out colony))
            {
                structureType = default(StructureType);
                return false;
            }

            if (!game.StructureTypes.TryGet(command.StructureTypeId, out structureType))
            {
                return false;
            }

            return true;
        }
    }
}