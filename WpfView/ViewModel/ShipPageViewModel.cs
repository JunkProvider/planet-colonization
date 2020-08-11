namespace SpaceLogistic.WpfView.ViewModel
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using SpaceLogistic.Core.CommandPattern;
    using SpaceLogistic.Core.Model;

    public sealed class ShipPageViewModel : IPageViewModel
    {
        public ShipPageViewModel(ICommandDispatcher commandDispatcher, Game game)
        {
            this.Ships = new ObservableCollection<ShipViewModel>(game.Ships.Select(s => new ShipViewModel(s, commandDispatcher)));
        }

        public string Title => "Ships";
        
        public ObservableCollection<ShipViewModel> Ships { get; }

        public void Update()
        {
            foreach (var ship in this.Ships)
            {
                ship.Update();
            }
        }
    }
}
