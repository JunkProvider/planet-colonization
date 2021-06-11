namespace SpaceLogistic.WpfView.Commands
{
    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.WpfView.View;

    public sealed class CloseOverlayCommandHandler : CommandHandlerBase<CloseOverlayCommand>
    {
        private readonly GameViewModel gameViewModel;

        public CloseOverlayCommandHandler(GameViewModel gameViewModel)
        {
            this.gameViewModel = gameViewModel;
        }

        public override bool CanExecute(CloseOverlayCommand command)
        {
            return this.gameViewModel.ActiveOverlay != null;
        }

        public override void Execute(CloseOverlayCommand command)
        {
            this.gameViewModel.CloseActiveOverlay();
        }
    }
}