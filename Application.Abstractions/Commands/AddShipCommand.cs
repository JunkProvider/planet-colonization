namespace SpaceLogistic.Application.Commands
{
    using System;

    public sealed class AddShipCommand
    {
        public AddShipCommand(Guid colonyId, Guid shipTypeId)
        {
            this.ColonyId = colonyId;
            this.ShipTypeId = shipTypeId;
        }

        public Guid ColonyId { get; }

        public Guid ShipTypeId { get; }
    }
}