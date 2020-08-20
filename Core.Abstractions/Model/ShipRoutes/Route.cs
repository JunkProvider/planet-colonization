namespace SpaceLogistic.Core.Model.ShipRoutes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Utility;

    public sealed class Route : IIdentity
    {
        private readonly List<RouteStop> stops;

        public Route(string name, IEnumerable<RouteStop> stops)
        {
            this.Name = name;
            this.stops = stops?.ToList() ?? throw new ArgumentNullException(nameof(stops));
        }

        public Guid Id { get; } = Guid.NewGuid();

        public string Name { get; set; }

        public IReadOnlyList<RouteStop> Stops => this.stops;

        public RouteStop FirstStop => this.stops.First();
        
        public OrbitalLocation GetNextDestination(OrbitalLocation currentLocation)
        {
            return this.GetNextStop(currentLocation)?.Location;
        }

        public IEnumerable<RouteStop> GetFutureStops(OrbitalLocation currentLocation)
        {
            var currentStopIndex = this.GetCurrentStopIndex(currentLocation);

            if (currentStopIndex < 0)
            {
                return Enumerable.Empty<RouteStop>();
            }

            return this.stops.Skip(currentStopIndex + 1).Concat(this.stops.Take(currentStopIndex + 1));
        }

        public RouteStop GetCurrentStop(OrbitalLocation currentLocation)
        {
            if (currentLocation == null)
            {
                return null;
            }

            return this.stops.FirstOrDefault(s => s.Location == currentLocation);
        }

        public RouteStop GetNextStop(OrbitalLocation currentLocation)
        {
            if (this.stops.Count == 0)
            {
                return null;
            }

            var currentLocationIndex = this.GetCurrentStopIndex(currentLocation);

            if (currentLocationIndex < 0)
            {
                return this.FirstStop;
            }

            if (this.stops.Count == 1)
            {
                return null;
            }

            var nextStopIndex = currentLocationIndex + 1;

            if (nextStopIndex >= this.stops.Count)
            {
                return this.FirstStop;
            }

            return this.stops[nextStopIndex];
        }

        public void AddStop(RouteStop stop)
        {
            this.stops.Add(stop);
        }

        public void RemoveStop(Guid stopId)
        {
            this.stops.RemoveWhere(s => s.Id == stopId);
        }

        private int GetCurrentStopIndex(OrbitalLocation currentLocation)
        {
            if (currentLocation == null)
            {
                return -1;
            }

            return this.stops.FindIndex(s => s.Location == currentLocation);
        }
    }
}
