namespace SpaceLogistic.Core.Model.ShipRoutes
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Utility;

    public sealed class Route : IIdentity
    {
        private readonly List<RouteStop> stops;

        public Route(string name, IEnumerable<RouteStop> stops)
            : this(Guid.NewGuid(), name, stops)
        {
        }

        public Route(Guid id, string name, IEnumerable<RouteStop> stops)
        {
            this.stops = stops.ToList();
            this.Id = id;
            this.Name = name;
        }

        public Guid Id { get; }

        public string Name { get; set; }

        public IReadOnlyList<RouteStop> Stops => this.stops;

        public RouteStop FirstStop => this.stops.First();
        
        public ILocation GetNextDestination(ILocation currentLocation)
        {
            return this.GetNextStop(currentLocation)?.Location;
        }

        public IEnumerable<RouteStop> GetFutureStops(ILocation currentLocation)
        {
            var currentStopIndex = this.GetCurrentStopIndex(currentLocation);

            if (currentStopIndex < 0)
            {
                return Enumerable.Empty<RouteStop>();
            }

            return this.stops.Skip(currentStopIndex + 1).Concat(this.stops.Take(currentStopIndex + 1));
        }

        public RouteStop GetCurrentStop(ILocation currentLocation)
        {
            if (currentLocation == null)
            {
                return null;
            }

            return this.stops.FirstOrDefault(s => s.Location == currentLocation);
        }

        public RouteStop GetNextStop(ILocation currentLocation)
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

        private int GetCurrentStopIndex(ILocation currentLocation)
        {
            if (currentLocation == null)
            {
                return -1;
            }

            return this.stops.FindIndex(s => s.Location == currentLocation);
        }
    }
}
