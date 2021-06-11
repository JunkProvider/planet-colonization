namespace SpaceLogistic.Persistence.Model.Celestials
{
    using System;

    using SpaceLogistic.Core.Model.Resources;
    using SpaceLogistic.Core.Model.Stations;
    using SpaceLogistic.Persistence.Model.Stations;

    public sealed class OrbitalLocationData
    {
        public OrbitalLocationData(Guid id, string name, double orbit, TimeSpan period, ColonyData colony)
        {
            this.Id = id;
            this.Name = name;
            this.Orbit = orbit;
            this.Period = period;
            this.Colony = colony;
        }
        
        public Guid Id { get; }

        public string Name { get; }

        public double Orbit { get; }

        public TimeSpan Period { get; }
        
        public ColonyData Colony { get; }
        
        public override string ToString()
        {
            return this.Name;
        }
    }
}
