namespace SpaceLogistic.Core.Model.Celestials
{
    using System;

    public sealed class OrbitalLocation : IIdentity
    {
        public OrbitalLocation(string name, double orbit, TimeSpan period)
        {
            this.Name = name;
            this.Orbit = orbit;
            this.Period = period;
        }

        public Guid Id { get; } = Guid.NewGuid();

        public string Name { get; }

        public double Orbit { get; }

        public TimeSpan Period { get; }

        public CelestialSystem System { get; internal set; }

        public IStationary Object { get; private set; }

        public void SetObject(IStationary obj)
        {
            this.Object = obj;
            obj.Location = this;
        }

        public override string ToString()
        {
            if (this.Object == null)
            {
                return $"{this.Name}";
            }

            return $"{this.Name} - {this.Object}";
        }
    }
}
