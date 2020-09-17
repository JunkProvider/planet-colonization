namespace SpaceLogistic.Application.Commands
{
    using System;

    public sealed class AddRouteStopCommand
    {
        public AddRouteStopCommand(Guid routeId, Guid locationId)
        {
            this.LocationId = locationId;
            this.RouteId = routeId;
        }

        public Guid RouteId { get; }

        public Guid LocationId { get; }
    }
}
