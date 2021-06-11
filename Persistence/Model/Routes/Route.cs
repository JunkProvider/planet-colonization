namespace SpaceLogistic.Persistence.Model.Routes
{
    using System;
    using System.Collections.Generic;

    public sealed class RouteData
    {
        public RouteData(Guid id, string name, List<RouteStopData> stops)
        {
            this.Id = id;
            this.Name = name;
            this.Stops = stops;
        }
        
        public Guid Id { get; }

        public string Name { get; set; }

        public List<RouteStopData> Stops { get; }
    }
}
