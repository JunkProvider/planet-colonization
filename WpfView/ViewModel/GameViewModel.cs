﻿namespace SpaceLogistic.WpfView.ViewModel
{
    using System.Collections.ObjectModel;
    using System.Windows;

    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.WpfView.ViewModel.Colonies;

    public sealed class GameViewModel : ViewModelBase
    {
        private readonly Game game;

        private IOverlayViewModel activeOverlay;

        private bool isOverlayActive;

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

        public bool IsOverlayActive
        {
            get => isOverlayActive;
            private set => SetProperty(ref isOverlayActive, value);
        }

        public IOverlayViewModel ActiveOverlay
        {
            get => activeOverlay;
            private set => SetProperty(ref activeOverlay, value);
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
