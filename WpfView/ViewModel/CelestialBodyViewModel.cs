namespace SpaceLogistic.WpfView.ViewModel
{
    using System;
    using System.Linq;
    using System.Windows.Media;

    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Utility.Units;

    public sealed class CelestialBodyViewModel
    {
        public CelestialBodyViewModel(CelestialSystem celestialSystem)
        {
            var celestialBody = celestialSystem.CentralBody;
            
            this.Name = celestialBody.Name;

            this.Orbit = $"Orbit: {GetOrbitValue(celestialSystem.Orbit, celestialBody.CelestialBodyType)}";

            var lowOrbitLocation = celestialSystem.OrbitalLocations.FirstOrDefault();

            if (lowOrbitLocation == null)
            {
                return;
            }
            
            this.EscapeVelocity = $"Esc. Velocity: {Math.Sqrt((2 * celestialBody.GravitationalParameter) / (celestialBody.Radius * 1000)):0} m/s";

            this.DisplayDiameter = GetIconSize(celestialBody.Diameter);
        }

        public string Name { get; }

        public string Orbit { get; }

        public string EscapeVelocity { get; }

        public double DisplayDiameter { get; }

        public double DisplayRadius => this.DisplayDiameter / 2;

        private static string GetOrbitValue(double orbit, CelestialBodyType celestialBodyType)
        {
            if (orbit <= 0)
            {
                return "-";
            }

            return Distance.Kilometers(orbit).Format(GetOrbitValueUnit(celestialBodyType), 0);
        }

        private static DistanceUnit GetOrbitValueUnit(CelestialBodyType celestialBodyType)
        {
            switch (celestialBodyType)
            {
                case CelestialBodyType.Star:
                    return DistanceUnit.BillionMeter;
                case CelestialBodyType.Planet:
                    return DistanceUnit.BillionMeter;
                case CelestialBodyType.Moon:
                    return DistanceUnit.MillionMeter;
                default:
                    return DistanceUnit.Kilometer;
            }
        }

        private static double GetIconSize(double diameter)
        {
            return Math.Sqrt(Math.Sqrt(diameter)) * 2;

            /*if (diameter >= 100000)
            {
                return 35;
            }

            if (diameter > 250000)
            {
                return 30;
            }

            if (diameter > 10000)
            {
                return 20;
            }

            if (diameter > 2500)
            {
                return 15;
            }

            if (diameter > 100)
            {
                return 10;
            }

            return 5;*/
        }
    }
}