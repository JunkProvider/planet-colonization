﻿namespace SpaceLogistic.WpfView.View.Map
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

            this.Orbit = primary.System.IsRoot ? "-" : $"{(primary.System.Orbit / 1e9):0.00} mio km";

            this.Period = this.FormatPeriod(primary.System.IsRoot, primary.System.Period);
            
            this.Diameter = $"{(primary.Diameter / 1e3):0} km";

            this.Density = $"{(primary.Density / 1e3):0.00} g/cm³";

            this.SurfaceGravity = $"{primary.SurfaceGravity:0.00} m/s²";

            this.EscapeVelocity = $"{primary.EscapeVelocity:0} m/s";

            this.EscapeVelocities = primary.EscapeVelocities.Select(v => $"{v.Value:0} m/s -> {v.Key}").ToList();
            
            this.Temperature = $"{(primary.Temperature - 273):0.00} °C";

            this.Resources = primary.Resources.Items.ToList();
        }

        public string Name { get; }

        public string Orbit { get; }

        public string Period { get; }

        public string Diameter { get; }
        
        public string Density { get; }

        public string SurfaceGravity { get; }

        public string EscapeVelocity { get; }

        public IReadOnlyCollection<string> EscapeVelocities { get; }
        
        public string Temperature { get; }

        public IReadOnlyCollection<Resource> Resources { get; }

        private string FormatPeriod(bool isRoot, TimeSpan period)
        {
            if (isRoot)
            {
                return "-";
            }

            if (period.TotalDays >= 366)
            {
                return $"{(period.TotalDays / 365.25):0.00} years ({period.TotalDays:0} days)";
            }

            if (period.TotalDays >= 1)
            {
                return $"{(period.TotalDays):0.00} days";
            }

            return $"{(period.TotalHours):0} hours ({period.TotalDays:0.00} days)";
        }
    }
}