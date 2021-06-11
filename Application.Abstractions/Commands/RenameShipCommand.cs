namespace SpaceLogistic.Application.Commands
{
    using System;

    public sealed class RenameShipCommand
    {
        public RenameShipCommand(Guid shipId, string newName)
        {
            this.ShipId = shipId;
            this.NewName = newName;
        }

        public Guid ShipId { get; }

        public string NewName { get; }
    }
}