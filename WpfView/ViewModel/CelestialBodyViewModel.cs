namespace SpaceLogistic.WpfView.ViewModel
{
    using System;
    using System.Linq;
    using System.Windows.Input;

    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Utility.Units;
    using SpaceLogistic.WpfView.Utility;

    public sealed class CelestialBodyViewModel : IIdentity
    {
        public CelestialBodyViewModel(CelestialSystem celestialSystem, DelegateCommand<CelestialBodyViewModel> selectCommand)
        {
            var celestialBody = celestialSystem.CentralBody;

            this.Id = celestialBody.Id;

            this.Name = celestialBody.Name;

            this.Orbit = $"Orbit: {GetOrbitValue(celestialSystem.Orbit, celestialBody.CelestialBodyType)}";

            var lowOrbitLocation = celestialSystem.OrbitalLocations.FirstOrDefault();

            if (lowOrbitLocation == null)
            {
                return;
            }
            
            this.EscapeVelocity = $"Esc. Velocity: {Math.Sqrt((2 * celestialBody.GravitationalParameter) / (celestialBody.Radius)):0} m/s";

            this.DisplayDiameter = GetIconSize(celestialBody.Diameter);
            this.SelectCommand = selectCommand;
        }

        public Guid Id { get; }

        public string Name { get; }

        public string Orbit { get; }

        public string EscapeVelocity { get; }

        public double DisplayDiameter { get; }

        public double DisplayRadius => this.DisplayDiameter / 2;

        public ICommand SelectCommand { get; }

        private static string GetOrbitValue(double orbit, CelestialBodyType celestialBodyType)
        {
            if (orbit <= 0)
            {
                return "-";
            }

            return Distance.Meter(orbit).Format(GetOrbitValueUnit(celestialBodyType), 0);
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
            return Math.Sqrt(Math.Sqrt(diameter / 1000)) * 2;
        }
    }
}