namespace SpaceLogistic.WpfView.View.Map
{
    using System;
    using System.Windows.Media;

    using SpaceLogistic.WpfView.Utility;

    public sealed class StationViewModel : MapItemViewModel
    {
        public StationViewModel(Guid id, double displayOrbit, double displayDiameter, double displayPeriod, DelegateCommand<Guid> selectCommand)
            : base(id, displayOrbit, displayDiameter, displayPeriod, selectCommand, new SolidColorBrush(Colors.White))
        {
        }
    }
}
