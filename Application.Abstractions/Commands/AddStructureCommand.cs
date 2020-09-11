namespace SpaceLogistic.Application.Commands
{
    using System;

    public sealed class AddStructureCommand
    {
        public AddStructureCommand(Guid colonyId, Guid structureTypeTypeId)
        {
            this.ColonyId = colonyId;
            this.StructureTypeId = structureTypeTypeId;
        }

        public Guid ColonyId { get; }

        public Guid StructureTypeId { get; }
    }
}