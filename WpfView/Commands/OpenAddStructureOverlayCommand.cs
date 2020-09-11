namespace SpaceLogistic.WpfView.Commands
{
    using System;

    public sealed class OpenAddStructureOverlayCommand
    {
        public OpenAddStructureOverlayCommand(Guid colonyId)
        {
            this.ColonyId = colonyId;
        }

        public Guid ColonyId { get; }
    }
}
