namespace SpaceLogistic.Application.Commands
{
    using System;

    public sealed class RemoveStructureCommand
    {
        public RemoveStructureCommand(Guid colonyId, Guid structureId)
        {
            this.ColonyId = colonyId;
            this.StructureId = structureId;
        }

        public Guid ColonyId { get; }

        public Guid StructureId { get; }
    }
}