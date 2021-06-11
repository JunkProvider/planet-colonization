namespace SpaceLogistic.Persistence.Model.Ships
{
    using SpaceLogistic.Core.Model.ShipRoutes;

    public sealed class ShipRefuelingProcessData
    {
        public ShipRefuelingProcessData(double totalTransferredFuel, RefuelBehavior refuelBehavior, double transferredFuel)
        {
            this.TotalTransferredFuel = totalTransferredFuel;
            this.RefuelBehavior = refuelBehavior;
            this.TransferredFuel = transferredFuel;
        }

        public double TotalTransferredFuel { get; }

        public RefuelBehavior RefuelBehavior { get; }

        public double TransferredFuel { get; set; }
    }
}