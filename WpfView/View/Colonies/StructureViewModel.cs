namespace SpaceLogistic.WpfView.ViewModel.Colonies
{
    using System;
    using System.Windows.Input;

    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Application.Commands;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.Structures;
    using SpaceLogistic.WpfView.Utility;

    public sealed class StructureViewModel : ViewModelBase, IIdentity
    {
        private readonly ICommandDispatcher commandDispatcher;

        private Guid colonyId;

        private Guid id;

        private string name;

        public StructureViewModel(ICommandDispatcher commandDispatcher)
        {
            this.commandDispatcher = commandDispatcher;
            this.RemoveCommand = new DelegateCommand(this.Remove, this.CanRemove);
        }
        
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

        public ICommand RemoveCommand { get; }

        public void Update(Guid colonyId, Structure structureModel)
        {
            this.colonyId = colonyId;
            this.Id = structureModel.Id;
            this.Name = structureModel.Name;
        }

        private bool CanRemove()
        {
            return this.commandDispatcher.CanExecute(new RemoveStructureCommand(this.colonyId, this.Id));
        }

        private void Remove()
        {
            this.commandDispatcher.Execute(new RemoveStructureCommand(this.colonyId, this.Id));
        }
    }
}
