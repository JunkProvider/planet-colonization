namespace SpaceLogistic.WpfView.Commands
{
    using SpaceLogistic.Application;
    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.WpfView.View;
    using SpaceLogistic.WpfView.View.Colonies;

    public sealed class OpenAddShipOverlayCommandHandler : CommandHandlerBase<OpenAddShipOverlayCommand>
    {
        private readonly IGameProvider game;

        private readonly GameViewModel gameViewModel;

        private readonly IViewModelFactory<AddShipOverlayViewModel> addShipOverlayViewModelFactory;

        public OpenAddShipOverlayCommandHandler(GameViewModel gameViewModel, IViewModelFactory<AddShipOverlayViewModel> addShipOverlayViewModelFactory, IGameProvider game)
        {
            this.gameViewModel = gameViewModel;
            this.addShipOverlayViewModelFactory = addShipOverlayViewModelFactory;
            this.game = game;
        }

        public override bool CanExecute(OpenAddShipOverlayCommand command)
        {
            return this.gameViewModel.ActiveOverlay == null;
        }

        public override void Execute(OpenAddShipOverlayCommand command)
        {
            var overlayViewModel = this.addShipOverlayViewModelFactory.Create();
            overlayViewModel.Update(command.ColonyId, this.game.Get().ShipTypes.GetAll());
            this.gameViewModel.SetActiveOverlay(overlayViewModel);
        }
    }
}