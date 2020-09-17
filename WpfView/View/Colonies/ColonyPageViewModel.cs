namespace SpaceLogistic.WpfView.ViewModel.Colonies
{
    using System.Windows.Input;

    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.Stations;
    using SpaceLogistic.WpfView.Commands;
    using SpaceLogistic.WpfView.Utility;

    public sealed class ColonyPageViewModel : ViewModelBase, IPageViewModel
    {
        private readonly ICommandDispatcher commandDispatcher;

        private readonly IViewModelFactory<ColonyViewModel> colonyViewModelFactory;
        
        private ColonyViewModel viewedColony;

        private string title;

        public ColonyPageViewModel(ICommandDispatcher commandDispatcher, IViewModelFactory<ColonyViewModel> colonyViewModelFactory)
        {
            this.colonyViewModelFactory = colonyViewModelFactory;
            this.commandDispatcher = commandDispatcher;

            this.MoveToNextColonyCommand = new DelegateCommand(this.MoveToNextColony);
        }

        public ColonyPageViewModel()
            : this(NullCommandDispatcher.Instance, NullViewModelFactory<ColonyViewModel>.Instance)
        {
            this.Title = this.ViewedColony.FullName;
        }

        public ICommand MoveToNextColonyCommand { get; }
        
        public string Title 
        {
            get => title;
            private set => SetProperty(ref title, value);
        }

        public ColonyViewModel ViewedColony
        {
            get => viewedColony;
            private set => SetProperty(ref viewedColony, value);
        }
        
        public void Update(Game game)
        {
            this.SetViewedColony(game, game.GetColonyOrDefault(this.ViewedColony?.Id));
        }

        public void SetViewedColony(Game game, Colony viewedColonyModel)
        {
            this.ViewedColony = ViewModelHelper.Update(
                this.ViewedColony,
                viewedColonyModel,
                colonyModel => this.colonyViewModelFactory.Create(),
                (colonyModel, colonyViewModel) => colonyViewModel.Update(
                    colonyModel, game.GetShipsAtLocation(colonyModel.Location)));
            this.Title = this.ViewedColony?.FullName ?? "Colonies";
        }

        public void ResetViewedColony()
        {
            this.ViewedColony = null;
        }

        private void MoveToNextColony()
        {
            this.commandDispatcher.Execute(new SwitchToNextColonyCommand());
        }
    }
}
