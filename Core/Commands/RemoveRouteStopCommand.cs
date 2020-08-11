namespace SpaceLogistic.Core.Commands
{
    using System;

    public sealed class RemoveRouteStopCommand
    {
        public RemoveRouteStopCommand(Guid routeId, Guid stopId)
        {
            this.RouteId = routeId;
            this.StopId = stopId;
        }

        public Guid RouteId { get; }

        public Guid StopId { get; }
    }
}
