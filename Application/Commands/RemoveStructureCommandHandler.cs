namespace SpaceLogistic.Application.Commands
{
    using System;

    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.Stations;
    using SpaceLogistic.Core.Model.Structures;

    public sealed class RemoveStructureCommandHandler : CommandHandlerBase<RemoveStructureCommand>
    {
        private readonly IGameProvider gameProvider;

        public RemoveStructureCommandHandler(IGameProvider gameProvider)
        {
            this.gameProvider = gameProvider;
        }

        public override bool CanExecute(RemoveStructureCommand command)
        {
            if (!this.TryGetEntities(command, out _, out _))
            {
                return false;
            }

            return true;
        }

        public override void Execute(RemoveStructureCommand command)
        {
            if (!this.TryGetEntities(command, out var colony, out _))
            {
                throw new InvalidOperationException("Can not execute command.");
            }

            colony.RemoveStructure(command.StructureId);
        }

        private bool TryGetEntities(RemoveStructureCommand command, out Colony colony, out Structure structure)
        {
            var game = this.gameProvider.Get();
            
            if (!game.TryGetColony(command.ColonyId, out colony))
            {
                structure = default(Structure);
                return false;
            }

            if (!colony.TryGetStructure(command.StructureId, out structure))
            {
                return false;
            }

            return true;
        }
    }
}