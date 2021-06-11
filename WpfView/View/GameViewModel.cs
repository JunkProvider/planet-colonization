namespace SpaceLogistic.WpfView.View
{
    using System.Collections.ObjectModel;
    using SpaceLogistic.Application;
    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.WpfView.View.Colonies;
    using SpaceLogistic.WpfView.View.Map;
    using SpaceLogistic.WpfView.View.Routes;
    using SpaceLogistic.WpfView.View.Ships;

    public sealed class GameViewModel : ViewModelBase
    {
        private readonly Game game;

        private IOverlayViewModel activeOverlay;

        private bool isOverlayActive;

        public GameViewModel(IGameProvider gameProvider, ICommandDispatcher commandDispatcher, ColonyPageViewModel colonyPageViewModel)
        {
            var game = this.game = gameProvider.Get();

            this.Pages = new ObservableCollection<IPageViewModel>(new IPageViewModel[]
                  {
                      new ShipPageViewModel(commandDispatcher, game),
                      new MapPageViewModel(game.CelestialSystem), 
                      new RoutePageViewModel(commandDispatcher, game),
                      colonyPageViewModel, 
                  });
        }

        public ObservableCollection<IPageViewModel> Pages { get; }

        public bool IsOverlayActive
        {
            get => this.isOverlayActive;
            private set => this.SetProperty(ref this.isOverlayActive, value);
        }

        public IOverlayViewModel ActiveOverlay
        {
            get => this.activeOverlay;
            private set => this.SetProperty(ref this.activeOverlay, value);
        }

        public void Update()
        {
            foreach (var page in this.Pages)
            {
                page.Update(this.game);
            }
        }

        public void SetActiveOverlay(IOverlayViewModel overlay)
        {
            this.ActiveOverlay = overlay;
            this.IsOverlayActive = overlay != null;
        }

        public void CloseActiveOverlay()
        {
            this.SetActiveOverlay(null);
        }
    }
}
