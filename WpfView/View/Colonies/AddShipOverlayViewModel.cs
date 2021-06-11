namespace SpaceLogistic.WpfView.View.Colonies
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Application.Commands;
    using SpaceLogistic.Core.Model.Ships;
    using SpaceLogistic.WpfView.Commands;
    using SpaceLogistic.WpfView.Utility;

    public sealed class AddShipOverlayViewModel : ViewModelBase, IOverlayViewModel
    {
        private readonly ICommandDispatcher commandDispatcher;

        private readonly IViewModelFactory<ShipTypeViewModel> shipTypeViewModelFactory;

        private Guid colonyId;

        private ObservableCollection<ShipTypeViewModel> shipTypes = new ObservableCollection<ShipTypeViewModel>();

        private ShipTypeViewModel selectedShipType;

        public AddShipOverlayViewModel(
            ICommandDispatcher commandDispatcher,
            IViewModelFactory<ShipTypeViewModel> shipTypeViewModelFactory)
        {
            this.commandDispatcher = commandDispatcher;
            this.shipTypeViewModelFactory = shipTypeViewModelFactory;

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

        public ObservableCollection<ShipTypeViewModel> ShipTypes
        {
            get => this.shipTypes;
            private set => this.SetProperty(ref this.shipTypes, value);
        }

        public ShipTypeViewModel SelectedShipType
        {
            get => this.selectedShipType;
            set => this.SetProperty(ref this.selectedShipType, value);
        }

        public void Update(Guid colonyId, IReadOnlyCollection<ShipType> shipTypes)
        {
            this.ColonyId = colonyId;
            
            this.ShipTypes = ViewModelHelper.UpdateCollectionByIdentity(
                this.shipTypes,
                shipTypes,
                _ => this.shipTypeViewModelFactory.Create(),
                (model, viewModel) => viewModel.Update(model));

            this.SelectedShipType = this.ShipTypes
                .FirstOrDefault(s => s.Id == this.selectedShipType?.Id);
        }

        private bool CanConfirm()
        {
            return this.SelectedShipType != null
                   && this.commandDispatcher.CanExecute(new AddShipCommand(this.ColonyId, this.SelectedShipType.Id));
        }

        private void Confirm()
        {
            this.commandDispatcher.Execute(new AddShipCommand(this.ColonyId, this.SelectedShipType?.Id ?? Guid.Empty));
            this.commandDispatcher.Execute(new CloseOverlayCommand());
        }

        private void Cancel()
        {
            this.commandDispatcher.Execute(new CloseOverlayCommand());
        }
    }
}