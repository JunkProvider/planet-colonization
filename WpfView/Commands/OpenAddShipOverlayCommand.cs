namespace SpaceLogistic.WpfView.Commands
{
    using System;

    public sealed class OpenAddShipOverlayCommand
    {
        public OpenAddShipOverlayCommand(Guid colonyId)
        {
            this.ColonyId = colonyId;
        }

        public Guid ColonyId { get; }
    }
}