namespace SpaceLogistic.WpfView.ViewModel
{
    using System;
    using System.Collections.ObjectModel;

    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.Stations;

    public sealed class ColonyViewModel : ViewModelBase, IIdentity
    {
        private readonly IViewModelFactory<StructureViewModel> structureViewModelFactory;

        private Guid id;

        private string name;

        private string fullName;

        private ObservableCollection<StructureViewModel> structures = new ObservableCollection<StructureViewModel>();

        public ColonyViewModel(IViewModelFactory<StructureViewModel> structureViewModelFactory)
        {
            this.structureViewModelFactory = structureViewModelFactory;
        }

        public ColonyViewModel()
            : this("Eden", "Earth - Eden")
        {
        }

        public ColonyViewModel(string name, string fullName)
            : this(NullViewModelFactory<StructureViewModel>.Instance)
        {
            this.name = name;
            this.fullName = fullName;
            this.structures.Add(new StructureViewModel("Deuterium Farm"));
            this.structures.Add(new StructureViewModel("Iron Mine"));
        }

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

        public void Update(Colony colony)
        {
            this.Id = colony.Id;
            this.Name = colony.Name;
            this.FullName = GetFullName(colony);

            this.Structures = ViewModelHelper.UpdateCollection(
                this.Structures,
                colony.Structures,
                structureModel => this.structureViewModelFactory.Create(),
                (structureModel, structureViewModel) => structureViewModel.Update(structureModel));
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