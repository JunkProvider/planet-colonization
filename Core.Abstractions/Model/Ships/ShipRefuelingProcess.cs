namespace SpaceLogistic.Core.Model
{
    using System;

    using SpaceLogistic.Core.Model.ShipRoutes;

    public sealed class ShipRefuelingProcess
    {
        public ShipRefuelingProcess(double totalTransferredFuel, RefuelBehavior refuelBehavior)
            : this(totalTransferredFuel, refuelBehavior, 0)
        {
        }

        public ShipRefuelingProcess(double totalTransferredFuel, RefuelBehavior refuelBehavior, double transferredFuel)
        {
            this.TotalTransferredFuel = totalTransferredFuel;
            this.RefuelBehavior = refuelBehavior;
            this.TransferredFuel = transferredFuel;
        }

        public double TotalTransferredFuel { get; }

        public RefuelBehavior RefuelBehavior { get; }

        public double TransferredFuel { get; private set; }

        public double RemainingTransferredFuel => this.TotalTransferredFuel - this.TransferredFuel;

        public double Progress => this.TransferredFuel / this.TotalTransferredFuel;

        public bool IsCompleted => this.TransferredFuel >= this.TotalTransferredFuel;

        public void UpdateProgress(double transferredFuel)
        {
            this.TransferredFuel += transferredFuel;
        }
    }
}