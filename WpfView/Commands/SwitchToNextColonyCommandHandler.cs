namespace SpaceLogistic.WpfView.Commands
{
    using System.Linq;

    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.WpfView.ViewModel;

    public sealed class SwitchToNextColonyCommandHandler : CommandHandlerBase<SwitchToNextColonyCommand>
    {
        private readonly Game game;

        private readonly GameViewModel gameViewModel;

        private readonly ColonyPageViewModel colonyPage;

        public SwitchToNextColonyCommandHandler(Game game, ColonyPageViewModel colonyPage, GameViewModel gameViewModel)
        {
            this.game = game;
            this.colonyPage = colonyPage;
            this.gameViewModel = gameViewModel;
        }

        public override bool CanExecute(SwitchToNextColonyCommand command)
        {
            return !this.gameViewModel.IsOverlayActive;
        }

        public override void Execute(SwitchToNextColonyCommand command)
        {
            var colonyModels = this.game.CelestialSystem.GetColonies().ToList();

            if (colonyModels.Count == 0)
            {
                this.colonyPage.ResetViewedColony();
                return;
            }

            var currentColonyId = this.colonyPage.ViewedColony?.Id;
            var currentColonyIndex = colonyModels.FindIndex(colony => colony.Id == currentColonyId);
            var nextColonyIndex = (currentColonyIndex + 1) % colonyModels.Count;
            var nextColonyModel = colonyModels[nextColonyIndex];

            this.colonyPage.SetViewedColony(nextColonyModel);
        }
    }
}
