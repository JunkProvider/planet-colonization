namespace SpaceLogistic.Core.Commands
{
    using System;

    public sealed class AddRouteStopCommand
    {
        public AddRouteStopCommand(Guid routeId, Guid orbitalLocationId)
        {
            this.OrbitalLocationId = orbitalLocationId;
            this.RouteId = routeId;
        }

        public Guid RouteId { get; }

        public Guid OrbitalLocationId { get; }
    }
}
