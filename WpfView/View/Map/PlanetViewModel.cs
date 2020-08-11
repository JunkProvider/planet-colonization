namespace SpaceLogistic.WpfView.View.Map
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;

    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Core.Model.Resources;
    using SpaceLogistic.WpfView.Utility;

    public sealed class PlanetViewModel : MapItemViewModel
    {
        public PlanetViewModel(Guid id, double displayOrbit, double displayDiameter, double displayPeriod, DelegateCommand<Guid> selectCommand, Brush brush, CelestialBody primary)
            : base(id, displayOrbit, displayDiameter, displayPeriod, selectCommand, brush)
        {
            this.Name = primary.Name;

            this.EscapeVelocity = $"{primary.EscapeVelocity:0} m/s";

            this.EscapeVelocities = primary.EscapeVelocities.Select(v => $"{v.Value:0} m/s -> {v.Key}").ToList();

            this.SurfaceGravity = $"{primary.SurfaceGravity:0.00} m/s²";

            this.Temperature = $"{(primary.Temperature - 273):0.00} °C";

            this.Resources = primary.Resources.ToList();
        }

        public string Name { get; }

        public string EscapeVelocity { get; }

        public IReadOnlyCollection<string> EscapeVelocities { get; }

        public string SurfaceGravity { get; }

        public string Temperature { get; }

        public IReadOnlyCollection<Resource> Resources { get; }
    }
}