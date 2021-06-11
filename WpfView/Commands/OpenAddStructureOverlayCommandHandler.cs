namespace SpaceLogistic.WpfView.Commands
{
    using SpaceLogistic.Application;
    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.WpfView.View;
    using SpaceLogistic.WpfView.View.Colonies;

    public sealed class OpenAddStructureOverlayCommandHandler : CommandHandlerBase<OpenAddStructureOverlayCommand>
    {
        private readonly IGameProvider game;

        private readonly GameViewModel gameViewModel;

        private readonly IViewModelFactory<AddStructureOverlayViewModel> addStructureOverlayViewModelFactory;

        public OpenAddStructureOverlayCommandHandler(GameViewModel gameViewModel, IViewModelFactory<AddStructureOverlayViewModel> addStructureOverlayViewModelFactory, IGameProvider game)
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
            overlayViewModel.Update(command.ColonyId, this.game.Get().StructureTypes.GetAll());
            this.gameViewModel.SetActiveOverlay(overlayViewModel);
        }
    }
}