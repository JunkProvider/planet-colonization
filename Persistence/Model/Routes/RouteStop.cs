namespace SpaceLogistic.Persistence.Model.Routes
{
    using System;
    using System.Collections.Generic;
    using SpaceLogistic.Core.Model.ShipRoutes;

    public sealed class RouteStopData
    {
        public RouteStopData(Guid id, Guid location, RefuelBehavior refuelBehavior, List<ItemTransferInstructionData> itemTransferInstructions)
        {
            this.Id = id;
            this.Location = location;
            this.RefuelBehavior = refuelBehavior;
            this.ItemTransferInstructions = itemTransferInstructions;
        }
        
        public Guid Id { get; }

        public Guid Location { get; }

        public RefuelBehavior RefuelBehavior { get; set; }

        public List<ItemTransferInstructionData> ItemTransferInstructions { get; }
    }
}