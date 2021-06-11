namespace SpaceLogistic.WpfView.View.Routes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Core.Model.Stations;
    using SpaceLogistic.WpfView.Utility;

    public sealed class CelestialSystemViewModel
    {
        private readonly CelestialSystem celestialSystem;

        public CelestialSystemViewModel(
            CelestialSystem celestialSystem, 
            DelegateCommand<StationViewModel> selectStationCommand,
            DelegateCommand<CelestialBodyViewModel> selectBodyCommand,
            DelegateCommand<CelestialSystemViewModel> clickCommand = null)
        {
            clickCommand = clickCommand ?? new DelegateCommand<CelestialSystemViewModel>(_ => { }, _ => false);

            this.ClickCommand = clickCommand;
            this.celestialSystem = celestialSystem;

            this.Name = celestialSystem.Name;
            this.DisplayOrbit = Math.Sqrt(Math.Sqrt(celestialSystem.Orbit)) * 8;

            this.CentralBody = new CelestialBodyViewModel(celestialSystem, selectBodyCommand);

            this.Stations = celestialSystem.OrbitalLocations
                .Select(p => p.Colony)
                .OfType<Station>()
                .Select(s => new StationViewModel(s, selectStationCommand))
                .ToList();
            
            this.Children = celestialSystem.Children.Select(c => new CelestialSystemViewModel(c, selectStationCommand, selectBodyCommand, clickCommand)).ToList();
        }

        public string Name { get; }

        public double DisplayOrbit { get; }

        public CelestialBodyViewModel CentralBody { get; }

        public IReadOnlyCollection<StationViewModel> Stations { get; }

        public IReadOnlyCollection<CelestialSystemViewModel> Children { get; }

        public ICommand ClickCommand { get; }

        public void Update()
        {
            foreach (var station in this.Stations)
            {
                station.Update();
            }

            foreach (var child in this.Children)
            {
                child.Update();   
            }
        }
    }
}
