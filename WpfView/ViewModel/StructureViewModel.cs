namespace SpaceLogistic.WpfView.ViewModel
{
    using System;

    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.Structures;

    public sealed class StructureViewModel : ViewModelBase, IIdentity
    {
        private Guid id;

        private string name;

        public StructureViewModel()
            : this("Deuterium Farm")
        {
        }

        public StructureViewModel(string name)
        {
            this.name = name;
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

        public void Update(Structure structureModel)
        {
            this.Id = structureModel.Id;
            this.Name = structureModel.Name;
        }
    }
}
