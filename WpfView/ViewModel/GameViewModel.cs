namespace SpaceLogistic.WpfView.ViewModel
{
    using System.Collections.ObjectModel;

    using SpaceLogistic.Core.CommandPattern;
    using SpaceLogistic.Core.Model;

    public sealed class GameViewModel : ViewModelBase
    {
        private readonly ICommandDispatcher commandDispatcher;

        private readonly Game game;

        public GameViewModel(Game game, ICommandDispatcher commandDispatcher)
        {
            this.game = game;
            this.commandDispatcher = commandDispatcher;

            this.Pages = new ObservableCollection<IPageViewModel>(new IPageViewModel[]
                  {
                      new ShipPageViewModel(commandDispatcher, game),
                      new MapPageViewModel(game.CelestialSystem), 
                      new RoutePageViewModel(commandDispatcher, game), 
                  });
        }

        public ObservableCollection<IPageViewModel> Pages { get; }
        
        public void Update()
        {
            foreach (var page in this.Pages)
            {
                page.Update();
            }
        }
    }
}
