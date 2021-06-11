namespace SpaceLogistic.WpfView.View.Colonies
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Application.Commands;
    using SpaceLogistic.Core.Model.Structures;
    using SpaceLogistic.WpfView.Commands;
    using SpaceLogistic.WpfView.Utility;

    public sealed class AddStructureOverlayViewModel : ViewModelBase, IOverlayViewModel
    {
        private readonly ICommandDispatcher commandDispatcher;

        private readonly IViewModelFactory<StructureTypeViewModel> structureTypeViewModelFactory;

        private Guid colonyId;

        private ObservableCollection<StructureTypeViewModel> structureTypes = new ObservableCollection<StructureTypeViewModel>();

        private StructureTypeViewModel selectedStructureType;

        public AddStructureOverlayViewModel(
            ICommandDispatcher commandDispatcher,
            IViewModelFactory<StructureTypeViewModel> structureTypeViewModelFactory)
        {
            this.commandDispatcher = commandDispatcher;
            this.structureTypeViewModelFactory = structureTypeViewModelFactory;

            this.ConfirmCommand = new DelegateCommand(this.Confirm, this.CanConfirm);
            this.CancelCommand = new DelegateCommand(this.Cancel);
        }
        
        public ICommand ConfirmCommand { get; }

        public ICommand CancelCommand { get; }

        public Guid ColonyId
        {
            get => this.colonyId;
            private set => this.SetProperty(ref this.colonyId, value);
        }

        public ObservableCollection<StructureTypeViewModel> StructureTypes
        {
            get => this.structureTypes;
            private set => this.SetProperty(ref this.structureTypes, value);
        }

        public StructureTypeViewModel SelectedStructureType
        {
            get => this.selectedStructureType;
            set => this.SetProperty(ref this.selectedStructureType, value);
        }

        public void Update(Guid colonyId, IReadOnlyCollection<StructureType> structureTypes)
        {
            this.ColonyId = colonyId;
            
            this.StructureTypes = ViewModelHelper.UpdateCollectionByIdentity(
                this.structureTypes,
                structureTypes,
                _ => this.structureTypeViewModelFactory.Create(),
                (model, viewModel) => viewModel.Update(model));

            this.SelectedStructureType = this.StructureTypes
                .FirstOrDefault(s => s.Id == this.selectedStructureType?.Id);
        }

        private bool CanConfirm()
        {
            return this.SelectedStructureType != null
                && this.commandDispatcher.CanExecute(new AddStructureCommand(this.ColonyId, this.SelectedStructureType.Id));
        }

        private void Confirm()
        {
            this.commandDispatcher.Execute(new AddStructureCommand(this.ColonyId, this.SelectedStructureType?.Id ?? Guid.Empty));
            this.commandDispatcher.Execute(new CloseOverlayCommand());
        }

        private void Cancel()
        {
            this.commandDispatcher.Execute(new CloseOverlayCommand());
        }
    }
}
