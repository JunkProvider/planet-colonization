namespace SpaceLogistic.WpfView.ViewModel.Colonies
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.Items;
    using SpaceLogistic.Core.Model.Resources;
    using SpaceLogistic.Core.Model.Ships;
    using SpaceLogistic.Core.Model.Stations;
    using SpaceLogistic.WpfView.Commands;
    using SpaceLogistic.WpfView.Utility;

    public sealed class ColonyViewModel : ViewModelBase, IIdentity
    {
        private readonly ICommandDispatcher commandDispatcher;

        private readonly IViewModelFactory<StructureViewModel> structureViewModelFactory;

        private readonly IViewModelFactory<ShipViewModel> shipViewModelFactory;

        private Guid id;

        private string name;

        private string fullName;

        private ObservableCollection<StructureViewModel> structures = new ObservableCollection<StructureViewModel>();

        private ObservableCollection<ShipViewModel> ships = new ObservableCollection<ShipViewModel>();

        private ObservableCollection<Item> items = new ObservableCollection<Item>();

        private ObservableCollection<Resource> resources = new ObservableCollection<Resource>();

        private bool canHaveResources;

        public ColonyViewModel(ICommandDispatcher commandDispatcher, IViewModelFactory<StructureViewModel> structureViewModelFactory, IViewModelFactory<ShipViewModel> shipViewModelFactory)
        {
            this.commandDispatcher = commandDispatcher;
            this.structureViewModelFactory = structureViewModelFactory;
            this.shipViewModelFactory = shipViewModelFactory;

            this.OpenAddStructureOverlayCommand = new DelegateCommand(
                this.OpenAddStructureOverlay,
                this.CanOpenAddStructureOverlay);

            this.OpenAddShipOverlayCommand = new DelegateCommand(
                this.OpenAddShipOverlay,
                this.CanOpenAddShipOverlay);
        }

        public ColonyViewModel(IViewModelFactory<ShipViewModel> shipViewModelFactory)
            : this("Eden", "Earth - Eden", shipViewModelFactory)
        {
        }

        public ColonyViewModel(string name, string fullName, IViewModelFactory<ShipViewModel> shipViewModelFactory)
            : this(NullCommandDispatcher.Instance, NullViewModelFactory<StructureViewModel>.Instance, shipViewModelFactory)
        {
            this.name = name;
            this.fullName = fullName;
            this.structures.Add(new StructureViewModel("Deuterium Farm"));
            this.structures.Add(new StructureViewModel("Iron Mine"));
        }

        public ICommand OpenAddStructureOverlayCommand { get; }

        public ICommand OpenAddShipOverlayCommand { get; }

        public Guid Id  
        {
            get => id;
            private set => SetProperty(ref id, value);
        }

        public string Name
        {
            get => name;
            private set => SetProperty(ref name, value);
        }

        public string FullName
        {
            get => fullName;
            private set => SetProperty(ref fullName, value);
        }

        public ObservableCollection<StructureViewModel> Structures
        {
            get => structures;
            private set => SetProperty(ref structures, value);
        }

        public ObservableCollection<ShipViewModel> Ships
        {
            get => ships;
            private set => SetProperty(ref ships, value);
        }

        public ObservableCollection<Item> Items
        {
            get => items;
            private set => SetProperty(ref items, value);
        }

        public bool CanHaveResources
        {
            get => canHaveResources;
            private set => SetProperty(ref canHaveResources, value);
        }

        public ObservableCollection<Resource> Resources
        {
            get => resources;
            private set => SetProperty(ref resources, value);
        }

        public void Update(Colony colony, IReadOnlyCollection<Ship> ships)
        {
            this.Id = colony.Id;
            this.Name = colony.Name;
            this.FullName = GetFullName(colony);

            this.Structures = ViewModelHelper.UpdateCollectionByIdentity(
                this.Structures,
                colony.Structures,
                structureModel => this.structureViewModelFactory.Create(),
                (structureModel, structureViewModel) => structureViewModel.Update(this.Id, structureModel));
            
            this.Ships = ViewModelHelper.UpdateCollectionByIdentity(
                this.Ships,
                ships,
                shipModel => this.shipViewModelFactory.Create(), 
                (shipModel, shipViewModel) => shipViewModel.Update(shipModel));

            this.Items = ViewModelHelper.UpdateCollectionByReference(
                this.Items,
                colony.Warehouse.Items,
                itemModel => itemModel,
                (itemModel, itemViewModel) => { });

            if (colony is Base @base)
            {
                this.Resources = ViewModelHelper.UpdateCollectionByReference(
                    this.Resources,
                    @base.Location.Resources.Items,
                    resourceModel => resourceModel,
                    (resourceModel, resourceViewModel) => { });
                this.CanHaveResources = true;
            }
            else
            {
                this.Resources.Clear();
                this.CanHaveResources = false;
            }
        }

        private bool CanOpenAddStructureOverlay()
        {
            return this.commandDispatcher.CanExecute(new OpenAddStructureOverlayCommand(this.Id));
        }

        private void OpenAddStructureOverlay()
        {
            this.commandDispatcher.Execute(new OpenAddStructureOverlayCommand(this.Id));
        }

        private bool CanOpenAddShipOverlay()
        {
            return this.commandDispatcher.CanExecute(new OpenAddShipOverlayCommand(this.Id));
        }

        private void OpenAddShipOverlay()
        {
            this.commandDispatcher.Execute(new OpenAddShipOverlayCommand(this.Id));
        }

        private static string GetFullName(Colony colony)
        {
            switch (colony)
            {
                case Base @base:
                    return $"{@base.Location.Name} - {@base.Name}";
                case Station station:
                    return $"{station.Location.Name} - {station.Name}";
                default:
                    return colony.Name;
            }
        }
    }
}