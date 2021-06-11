namespace SpaceLogistic.WpfView.View.Map
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;
    using System.Windows.Media;
    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.WpfView.Utility;

    public class MapItemViewModel : ViewModelBase
    {
        public static IEnumerable<MapItemViewModel> CreateRoot(
            CelestialSystem primarySystem, 
            DelegateCommand<Guid> selectCommand)
        {
            yield return new PlanetViewModel(
                primarySystem.Id,
                0,
                GetDisplayDiameter(primarySystem.CentralBody.Diameter),
                0,
                selectCommand,
                new SolidColorBrush(primarySystem.Color.ToMediaColor()),
                primarySystem.CentralBody);
            
            var planetIndex = 0;
            foreach (var subSystem in primarySystem.Children)
            {
                yield return new PlanetViewModel(
                    subSystem.Id,
                    GetDisplayOrbit(planetIndex, subSystem.Orbit),
                    GetDisplayDiameter(subSystem.CentralBody.Diameter),
                    GetDisplayPeriod(subSystem.Period),
                    selectCommand,
                    new SolidColorBrush(subSystem.Color.ToMediaColor()),
                    subSystem.CentralBody);

                planetIndex++;
            }

            var stationIndex = 0;
            foreach (var location in primarySystem.OrbitalLocations)
            {
                if (location.Colony == null)
                {
                    continue;
                }

                yield return new StationViewModel(
                    location.Id,
                    GetDisplayOrbit(stationIndex, location.Orbit),
                    5,
                    GetDisplayPeriod(location.Period),
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

        public virtual void Update()
        {

        }

        private static double GetDisplayOrbit(int index, double orbit)
        {
            return ((index + 1) * 25) + (Math.Sqrt(Math.Sqrt(orbit)) * 5);
        }

        private static double GetDisplayDiameter(double diameter)
        {
            return Math.Sqrt(Math.Sqrt(diameter)) * 2;
        }

        private static double GetDisplayPeriod(TimeSpan period)
        {
            return Math.Sqrt(period.TotalDays) * 50;
        }
    }
}
