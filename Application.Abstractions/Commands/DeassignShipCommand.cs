namespace SpaceLogistic.Application.Commands
{
    using System;

    public sealed class DeassignShipCommand
    {
        public DeassignShipCommand(Guid shipId)
        {
            this.ShipId = shipId;
        }
        
        public Guid ShipId { get; }
    }
}