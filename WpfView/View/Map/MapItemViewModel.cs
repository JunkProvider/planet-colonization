namespace SpaceLogistic.WpfView.View.Map
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;
    using System.Windows.Media;

    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Core.Model.Stations;
    using SpaceLogistic.WpfView.Utility;
    using SpaceLogistic.WpfView.ViewModel;

    public class MapItemViewModel : ViewModelBase
    {
        public static IEnumerable<MapItemViewModel> CreateRoot(
            CelestialSystem primarySystem, 
            DelegateCommand<Guid> selectCommand)
        {
            yield return new PlanetViewModel(
                primarySystem.Id,
                0,
                Math.Sqrt(Math.Sqrt(primarySystem.CentralBody.Diameter)) * 2,
                0,
                selectCommand,
                new SolidColorBrush(primarySystem.Color.ToMediaColor()),
                primarySystem.CentralBody);
            
            var planetIndex = 0;
            foreach (var subSystem in primarySystem.Children)
            {
                yield return new PlanetViewModel(
                    subSystem.Id,
                    (planetIndex + 1) * 25 + Math.Sqrt(Math.Sqrt(subSystem.Orbit)) * 5,
                    Math.Sqrt(Math.Sqrt(subSystem.CentralBody.Diameter)) * 2,
                    Math.Sqrt(subSystem.Period.TotalDays) * 50,
                    selectCommand,
                    new SolidColorBrush(subSystem.Color.ToMediaColor()),
                    subSystem.CentralBody);

                planetIndex++;
            }

            var stationIndex = 0;
            foreach (var location in primarySystem.OrbitalLocations)
            {
                if (!(location.Object is Station))
                {
                    continue;
                }

                yield return new StationViewModel(
                    location.Id,
                    (stationIndex + 1) * 25 + Math.Sqrt(Math.Sqrt(location.Orbit)) * 5,
                    5,
                    Math.Sqrt(location.Period.TotalDays) * 50,
                    new DelegateCommand<Guid>(_ => { }));

                stationIndex++;
            }
        }

        public MapItemViewModel(Guid id, double displayOrbit, double displayDiameter, double displayPeriod, DelegateCommand<Guid> selectCommand, Brush brush)
        {
            this.Id = id;
            this.DisplayOrbit = displayOrbit;
            this.DisplayDiameter = displayDiameter;
            this.DisplayPeriod = displayPeriod;
            this.SurfaceBrush = brush;
            this.SelectCommand = new DelegateCommand(() => selectCommand.Execute(this.Id));
        }

        public Guid Id { get; }
        
        public double DisplayOrbit { get; }

        public double DisplayDiameter { get; }

        public double DisplayPeriod { get; }

        public Brush SurfaceBrush { get; }

        public ICommand SelectCommand { get; }
    }
}
