namespace SpaceLogistic.WpfView.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.Items;
    using SpaceLogistic.Core.Model.Resources;
    using SpaceLogistic.Core.Model.Stations;
    using SpaceLogistic.WpfView.Commands;
    using SpaceLogistic.WpfView.Utility;

    public sealed class ColonyViewModel : ViewModelBase, IIdentity
    {
        private readonly ICommandDispatcher commandDispatcher;

        private readonly IViewModelFactory<StructureViewModel> structureViewModelFactory;

        private Guid id;

        private string name;

        private string fullName;

        private ObservableCollection<StructureViewModel> structures = new ObservableCollection<StructureViewModel>();

        private ObservableCollection<Item> items = new ObservableCollection<Item>();

        private ObservableCollection<Resource> resources = new ObservableCollection<Resource>();

        private bool canHaveResources;

        public ColonyViewModel(ICommandDispatcher commandDispatcher, IViewModelFactory<StructureViewModel> structureViewModelFactory)
        {
            this.commandDispatcher = commandDispatcher;
            this.structureViewModelFactory = structureViewModelFactory;

            this.OpenAddStructureOverlayCommand = new DelegateCommand(
                this.OpenAddStructureOverlay,
                this.CanOpenAddStructureOverlay);
        }

        public ColonyViewModel()
            : this("Eden", "Earth - Eden")
        {
        }

        public ColonyViewModel(string name, string fullName)
            : this(NullCommandDispatcher.Instance, NullViewModelFactory<StructureViewModel>.Instance)
        {
            this.name = name;
            this.fullName = fullName;
            this.structures.Add(new StructureViewModel("Deuterium Farm"));
            this.structures.Add(new StructureViewModel("Iron Mine"));
        }

        public ICommand OpenAddStructureOverlayCommand { get; }

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

        public void Update(Colony colony)
        {
            this.Id = colony.Id;
            this.Name = colony.Name;
            this.FullName = GetFullName(colony);

            this.Structures = ViewModelHelper.UpdateCollectionByIdentity(
                this.Structures,
                colony.Structures,
                structureModel => this.structureViewModelFactory.Create(),
                (structureModel, structureViewModel) => structureViewModel.Update(this.Id, structureModel));

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