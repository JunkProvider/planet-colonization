namespace SpaceLogistic.WpfView.Commands
{
    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.WpfView.ViewModel;

    public sealed class OpenAddShipOverlayCommandHandler : CommandHandlerBase<OpenAddShipOverlayCommand>
    {
        private readonly Game game;

        private readonly GameViewModel gameViewModel;

        private readonly IViewModelFactory<AddShipOverlayViewModel> addShipOverlayViewModelFactory;

        public OpenAddShipOverlayCommandHandler(GameViewModel gameViewModel, IViewModelFactory<AddShipOverlayViewModel> addShipOverlayViewModelFactory, Game game)
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
            overlayViewModel.Update(command.ColonyId, this.game.ShipTypes.GetAll());
            this.gameViewModel.SetActiveOverlay(overlayViewModel);
        }
    }
}