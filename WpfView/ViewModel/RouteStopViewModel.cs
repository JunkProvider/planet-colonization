namespace SpaceLogistic.WpfView.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.Items;
    using SpaceLogistic.Core.Model.ShipRoutes;
    using SpaceLogistic.WpfView.Utility;

    public sealed class RouteStopViewModel : ViewModelBase, IIdentity
    {
        private readonly ItemTypes itemTypes;

        private readonly RouteStop stop;

        private RefuelBehaviorOption selectedRefuelBehavior;

        private ObservableCollection<LoadInstructionViewModel> loadInstructions;

        private ObservableCollection<LoadInstructionViewModel> unloadInstructions;

        public RouteStopViewModel(ItemTypes itemTypes, RouteStop stop, Action<Guid> deleteDelegate)
        {
            this.itemTypes = itemTypes;
            this.stop = stop;
            
            this.Name = stop.Location.Name;
            
            this.LoadInstructions = new ObservableCollection<LoadInstructionViewModel>(stop.LoadInstructions.Select(this.CreateLoadInstructionViewModel));
            this.UnloadInstructions = new ObservableCollection<LoadInstructionViewModel>(stop.UnloadInstructions.Select(this.CreateLoadInstructionViewModel));
            this.AddLoadInstructionCommand = new DelegateCommand(this.AddLoadInstruction);
            this.AddUnloadInstructionCommand = new DelegateCommand(this.AddUnloadInstruction);
            this.DeleteCommand = new DelegateCommand(() => deleteDelegate(this.Id));

            this.Update();
        }

        public Guid Id => this.stop.Id;

        public string Name { get; }

        public IReadOnlyCollection<RefuelBehaviorOption> SelectableRefuelBehaviors { get; } = new List<RefuelBehaviorOption>(RefuelBehaviorOption.All);

        public RefuelBehaviorOption SelectedRefuelBehavior
        {
            get => this.selectedRefuelBehavior;
            set
            {
                if (value == this.selectedRefuelBehavior)
                {
                    return;
                }

                this.selectedRefuelBehavior = value;
                this.OnRefuelBehaviorSelected(value.Value);
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<LoadInstructionViewModel> UnloadInstructions
        {
            get => unloadInstructions;
            private set => SetProperty(ref unloadInstructions, value);
        }

        public ObservableCollection<LoadInstructionViewModel> LoadInstructions
        {
            get => this.loadInstructions;
            private set => this.SetProperty(ref this.loadInstructions, value);
        }

        public ICommand DeleteCommand { get; }

        public ICommand AddLoadInstructionCommand { get; }

        public ICommand AddUnloadInstructionCommand { get; }

        public void Update()
        {
            this.SetSelectedRefuelBehavior(this.stop.RefuelBehavior);

            this.LoadInstructions = ViewModelHelper.Update(
                this.LoadInstructions,
                this.stop.LoadInstructions,
                this.CreateLoadInstructionViewModel,
                (loadInstruction, viewModel) => viewModel.Update());

            this.UnloadInstructions = ViewModelHelper.Update(
                this.UnloadInstructions,
                this.stop.UnloadInstructions,
                this.CreateLoadInstructionViewModel,
                (loadInstruction, viewModel) => viewModel.Update());
        }

        private void OnRefuelBehaviorSelected(RefuelBehavior refuelBehavior)
        {
            // TODO: Move to command
            this.stop.RefuelBehavior = refuelBehavior;
        }

        private void SetSelectedRefuelBehavior(RefuelBehavior refuelBehavior)
        {
            var option = this.SelectableRefuelBehaviors.First(o => o.Value == stop.RefuelBehavior);

            if (option == this.selectedRefuelBehavior)
            {
                return;
            }

            this.selectedRefuelBehavior = option;
            this.OnPropertyChanged(nameof(this.SelectedRefuelBehavior));
        }

        private void AddLoadInstruction()
        {
            // TODO: Command
            var loadInstruction = new LoadInstruction(this.itemTypes.GetAll().First(), 40);
            this.stop.AddLoadInstruction(loadInstruction);
        }

        private void AddUnloadInstruction()
        {
            // TODO: Command
            var loadInstruction = new UnloadInstruction(this.itemTypes.GetAll().First(), 0);
            this.stop.AddUnloadInstruction(loadInstruction);
        }

        private void DeleteLoadInstruction(Guid instructionId)
        {
            this.stop.RemoveLoadInstruction(instructionId);
        }

        private void DeleteUnloadInstruction(Guid instructionId)
        {
            this.stop.RemoveUnloadInstruction(instructionId);
        }

        private LoadInstructionViewModel CreateLoadInstructionViewModel(LoadInstruction instruction)
        {
            return new LoadInstructionViewModel(this.itemTypes, instruction, true, this.DeleteLoadInstruction);
        }

        private LoadInstructionViewModel CreateLoadInstructionViewModel(UnloadInstruction instruction)
        {
            return new LoadInstructionViewModel(this.itemTypes, instruction, false, this.DeleteUnloadInstruction);
        }
    }
}