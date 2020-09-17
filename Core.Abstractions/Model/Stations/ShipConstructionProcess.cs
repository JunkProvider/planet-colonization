namespace SpaceLogistic.Core.Model.Stations
{
    using System;

    using SpaceLogistic.Core.Model.Ships;

    public sealed class ShipConstructionProcess : TimedProcess
    {
        public ShipConstructionProcess(TimeSpan totalTime, ShipType shipType, Colony colony)
            : base(totalTime)
        {
            this.ShipType = shipType;
            this.Colony = colony;
        }

        public ShipType ShipType { get; }

        public Colony Colony { get; }
    }
}
