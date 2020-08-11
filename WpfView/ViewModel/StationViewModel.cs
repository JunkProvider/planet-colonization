namespace SpaceLogistic.WpfView.ViewModel
{
    using System;
    using System.Windows.Input;

    using SpaceLogistic.Core.Model.Stations;
    using SpaceLogistic.WpfView.Utility;

    public sealed class StationViewModel : ViewModelBase
    {
        private readonly Station station;

        private string storedFuel;

        public StationViewModel(Station station, DelegateCommand<StationViewModel> selectCommand)
        {
            this.station = station;
            this.Name = station.Name;
            this.Orbit = $"Orbit: {(station.Location.Orbit / 1e3):0.0} M";
            this.Warehouse = new StorageViewModel(station.Warehouse);
            this.SelectCommand = selectCommand;
        }

        public Guid Id => this.station.Id;
        
        public string Name { get; }

        public string Orbit { get; }

        public string StoredFuel
        {
            get => this.storedFuel;
            private set
            {
                if (value == this.storedFuel)
                {
                    return;
                }

                this.storedFuel = value;
                this.OnPropertyChanged();
            }
        }

        public StorageViewModel Warehouse { get; }

        public ICommand SelectCommand { get; }

        public void Update()
        {
            this.StoredFuel = this.station.FuelStorageCapacity > 0
                ? $"Stored Fuel: {this.station.StoredFuel:0.0} / {this.station.FuelStorageCapacity:0.0}"
                : $"Stored Fuel: none";

            this.Warehouse.Update();
        }
    }
}
