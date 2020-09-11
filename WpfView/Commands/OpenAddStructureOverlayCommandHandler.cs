namespace SpaceLogistic.WpfView.Commands
{
    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.WpfView.ViewModel;

    public sealed class OpenAddStructureOverlayCommandHandler : CommandHandlerBase<OpenAddStructureOverlayCommand>
    {
        private readonly Game game;

        private readonly GameViewModel gameViewModel;

        private readonly IViewModelFactory<AddStructureOverlayViewModel> addStructureOverlayViewModelFactory;

        public OpenAddStructureOverlayCommandHandler(GameViewModel gameViewModel, IViewModelFactory<AddStructureOverlayViewModel> addStructureOverlayViewModelFactory, Game game)
        {
            this.gameViewModel = gameViewModel;
            this.addStructureOverlayViewModelFactory = addStructureOverlayViewModelFactory;
            this.game = game;
        }

        public override bool CanExecute(OpenAddStructureOverlayCommand command)
        {
            return this.gameViewModel.ActiveOverlay == null;
        }

        public override void Execute(OpenAddStructureOverlayCommand command)
        {
            var overlayViewModel = this.addStructureOverlayViewModelFactory.Create();
            overlayViewModel.Update(command.ColonyId, this.game.StructureTypes.GetAll());
            this.gameViewModel.SetActiveOverlay(overlayViewModel);
        }
    }
}