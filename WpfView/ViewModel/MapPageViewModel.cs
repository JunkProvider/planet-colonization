namespace SpaceLogistic.WpfView.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.WpfView.Utility;
    using SpaceLogistic.WpfView.View.Map;

    public sealed class MapPageViewModel : ViewModelBase, IPageViewModel
    {
        private readonly CelestialSystem rootSystem;

        private IReadOnlyCollection<MapItemViewModel> mapItems;

        private Guid selectedItemId;

        private MapItemViewModel selectedMapItem;

        public MapPageViewModel(CelestialSystem celestialSystem)
        {
            this.rootSystem = celestialSystem;

            this.DeselectSystemCommand = new DelegateCommand(this.DeselectSystem);

            this.SelectSystem(this.rootSystem.Id);
        }

        public string Title => "Planets";

        public ICommand DeselectSystemCommand { get; }

        public IReadOnlyCollection<MapItemViewModel> MapItems
        {
            get => mapItems;
            private set => SetProperty(ref mapItems, value);
        }

        public MapItemViewModel SelectedMapItem
        {
            get => selectedMapItem;
            private set => SetProperty(ref selectedMapItem, value);
        }

        public void Update()
        {
        }

        private void SelectSystem(Guid selectedSystemId)
        {
            var selectedSystem = this.rootSystem.GetById(selectedSystemId);

            this.selectedItemId = selectedSystemId;
            
            this.MapItems = MapItemViewModel.CreateRoot(selectedSystem, new DelegateCommand<Guid>(this.SelectSystem))
                .ToList();

            this.SelectedMapItem = this.MapItems.FirstOrDefault();
        }

        private void DeselectSystem()
        {
            var parentId = this.rootSystem.GetById(this.selectedItemId).Parent?.Id;

            if (!parentId.HasValue)
            {
                return;
            }

            this.SelectSystem(parentId.Value);
        }
    }
}
