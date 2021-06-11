using SpaceLogistic.Core.Model.Celestials;
using SpaceLogistic.Persistence.Model.Resources;

namespace SpaceLogistic.Persistence.Model.Celestials
{
    using System;
    using SpaceLogistic.Persistence.Model.Stations;

    public sealed class CelestialBodyData
    {
        public CelestialBodyData(Guid id, string name, CelestialBodyType celestialBodyType, double radius, double mass, double density, double gravitationalParameter, ResourceCollectionData resources, ColonyData colony)
        {
            this.Id = id;
            this.Name = name;
            this.CelestialBodyType = celestialBodyType;
            this.Radius = radius;
            this.Mass = mass;
            this.Density = density;
            this.GravitationalParameter = gravitationalParameter;
            this.Resources = resources;
            this.Colony = colony;
        }
        
        public Guid Id { get; set; }

        public string Name { get; set; }

        public CelestialBodyType CelestialBodyType { get; set; }

        public double Radius { get; set; }

        public double Mass { get; set; }

        /// <summary>
        /// In kg/m³
        /// </summary>
        public double Density { get; set; }

        public double GravitationalParameter { get; set; }

        public ResourceCollectionData Resources { get; set; }
        
        public ColonyData Colony { get; }
        
        public override string ToString()
        {
            return this.Name;
        }
    }
}