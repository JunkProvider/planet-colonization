﻿namespace SpaceLogistic.Core.Commands
{
    using System;

    public sealed class DeleteRouteCommand
    {
        public DeleteRouteCommand(Guid routeId)
        {
            this.RouteId = routeId;
        }

        public Guid RouteId { get; }
    }
}