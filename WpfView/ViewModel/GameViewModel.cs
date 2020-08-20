namespace SpaceLogistic.WpfView.ViewModel
{
    using System.Collections.ObjectModel;

    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Core.Model;

    public sealed class GameViewModel : ViewModelBase
    {
        private readonly Game game;

        public GameViewModel(Game game, ICommandDispatcher commandDispatcher, ColonyPageViewModel colonyPageViewModel)
        {
            this.game = game;

            this.Pages = new ObservableCollection<IPageViewModel>(new IPageViewModel[]
                  {
                      new ShipPageViewModel(commandDispatcher, game),
                      new MapPageViewModel(game.CelestialSystem), 
                      new RoutePageViewModel(commandDispatcher, game),
                      colonyPageViewModel, 
                  });
        }

        public ObservableCollection<IPageViewModel> Pages { get; }
        
        public void Update()
        {
            foreach (var page in this.Pages)
            {
                page.Update(this.game);
            }
        }
    }
}
