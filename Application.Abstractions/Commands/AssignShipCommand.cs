namespace SpaceLogistic.Application.Commands
{
    using System;

    public sealed class AssignShipCommand
    {
        public AssignShipCommand(Guid routeId, Guid shipId)
        {
            this.ShipId = shipId;
            this.RouteId = routeId;
        }

        public Guid RouteId { get; }

        public Guid ShipId { get; }
    }
}