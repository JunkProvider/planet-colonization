namespace SpaceLogistic.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using JPV.RocketScience;

    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Core.Services.WorldGeneration;

    public class TransferCalculator : ITransferCalculator
    {
        private readonly WorldSettings settings;

        public TransferCalculator(WorldSettings settings)
        {
            this.settings = settings;
        }

        public void Calculate(
            OrbitalLocation origin,
            OrbitalLocation destination,
            double dryMass,
            out double fuelCosts,
            out TimeSpan travelTime)
        {
            this.CalculateTransferOrbit(origin, destination, out var deltaV, out var period);

            var exhaustMass = Physics.GetRequiredExhaustMass(deltaV, dryMass, this.settings.ShipSpecificImpulse);
            
            fuelCosts = exhaustMass / this.settings.ItemMass;

            travelTime = TimeSpan.FromMilliseconds(period.TotalMilliseconds / this.settings.TravelSpeedFactor);
        }

        public void CalculateTransferOrbit(
            OrbitalLocation origin,
            OrbitalLocation destination,
            out double deltaV,
            out TimeSpan period)
        {
            var plan = TransferPlan.Create(origin, destination);
            this.CalculateTransferOrbit(plan, out deltaV, out period);
        }

        public void CalculateTransferOrbit(
            TransferPlan plan,
            out double deltaV,
            out TimeSpan period)
        {
            Transfer transfer;

            if (plan.OriginSystem == plan.PrimarySystem)
            {
                transfer = this.CalculateTransferOrbitFromPrimaryToSatellite(plan);
            }
            else if (plan.DestinationSystem == plan.PrimarySystem)
            {
                transfer = this.CalculateTransferOrbitFromSatelliteToPrimary(plan);
            }
            else
            {
                transfer = this.CalculateTransferOrbitBetweenSatellites(plan);
            }
            
            deltaV = transfer.TotalDeltaVelocity;
            period = transfer.Period;
        }

        private Transfer CalculateTransferOrbitFromPrimaryToSatellite(TransferPlan plan)
        {
            var primaryGravitationalParameter = plan.PrimarySystem.CentralBody.GravitationalParameter;

            this.CalculateEscapeTrajectory(
                plan.DestinationSubsystems,
                plan.DestinationOrbit,
                out var arrivalEscapeDeltaV,
                out var arrivalOrbit);

            var transfer = HohmannTransfer2.PrimaryToSatellite(
                primaryGravitationalParameter,
                plan.OriginOrbit,
                new Satellite(arrivalOrbit * 1000, plan.DestinationSystem.CentralBody.GravitationalParameter),
                arrivalOrbit);

            return new Transfer(transfer.InsertionDeltaVelocity, arrivalEscapeDeltaV + transfer.CaptureDeltaVelocity, transfer.Period);
        }

        private Transfer CalculateTransferOrbitFromSatelliteToPrimary(TransferPlan plan)
        {
            var primaryGravitationalParameter = plan.PrimarySystem.CentralBody.GravitationalParameter;

            this.CalculateEscapeTrajectory(
                plan.OriginSubsystems,
                plan.OriginOrbit,
                out var departureEscapeDeltaV,
                out var departureOrbit);

            var transfer = HohmannTransfer2.PrimaryToSatellite(
                primaryGravitationalParameter,
                plan.OriginOrbit,
                new Satellite(departureOrbit * 1000, plan.DestinationSystem.CentralBody.GravitationalParameter),
                departureOrbit);

            return new Transfer(transfer.InsertionDeltaVelocity, departureEscapeDeltaV + transfer.CaptureDeltaVelocity, transfer.Period);
        }

        private Transfer CalculateTransferOrbitBetweenSatellites(TransferPlan plan)
        {
            var originSystem = plan.OriginSystem;
            var destinationSystem = plan.DestinationSystem;

            var primaryGravitationalParameter = plan.PrimarySystem.CentralBody.GravitationalParameter;

            this.CalculateEscapeTrajectory(
                plan.OriginSubsystems,
                plan.OriginOrbit,
                out var departureEscapeDeltaV,
                out var departureOrbit);

            this.CalculateEscapeTrajectory(
                plan.DestinationSubsystems,
                plan.DestinationOrbit,
                out var arrivalEscapeDeltaV,
                out var arrivalOrbit);

            var transfer = HohmannTransfer2.BetweenSatellites(
                primaryGravitationalParameter,
                new Satellite(originSystem.Orbit * 1000, originSystem.CentralBody.GravitationalParameter),
                departureOrbit * 1000,
                new Satellite(destinationSystem.Orbit * 1000, destinationSystem.CentralBody.GravitationalParameter),
                arrivalOrbit * 1000);

            return new Transfer(departureEscapeDeltaV + transfer.InsertionDeltaVelocity, arrivalEscapeDeltaV + transfer.CaptureDeltaVelocity, transfer.Period);
        }

        private void CalculateEscapeTrajectory(
            IReadOnlyCollection<CelestialSystem> subSystems,
            double orbitRelativeToSubSystem,
            out double deltaV,
            out double orbit)
        {
            var currentOrbit = orbitRelativeToSubSystem;

            var totalEscapeVelocity = 0d;
            var totalOrbit = currentOrbit;

            foreach (var currentSystem in subSystems)
            {
                var currentBody = currentSystem.CentralBody;
                var currentEscapeDeltaV = OrbitMechanics.GetEscapeDeltaVelocity(currentBody.GravitationalParameter, currentOrbit * 1000);

                totalEscapeVelocity += currentEscapeDeltaV;

                currentOrbit = currentSystem.Orbit;
                totalOrbit += currentSystem.Orbit;
            }

            deltaV = totalEscapeVelocity;
            orbit = totalOrbit;
        }
    }
}
