namespace SpaceLogistic.Core.Model.Stations
{
    using System;

    using SpaceLogistic.Core.Model.Ships;

    public sealed class ShipConstructionProcess : TimedProcess
    {
        public ShipConstructionProcess(TimeSpan totalTime, double progress, ShipType shipType, Colony colony)
            : base(totalTime, progress)
        {
            this.ShipType = shipType;
            this.Colony = colony;
        }

        public ShipType ShipType { get; }

        public Colony Colony { get; }
    }
}
