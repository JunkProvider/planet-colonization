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
            ILocation origin,
            ILocation destination,
            double dryMass,
            out double fuelCosts,
            out TimeSpan travelTime)
        {
            this.CalculateTransfer(origin, destination, out var deltaV, out var period);

            var exhaustMass = Physics.GetRequiredExhaustMass(deltaV, dryMass, this.settings.ShipSpecificImpulse);
            
            fuelCosts = exhaustMass / this.settings.ItemMass;

            travelTime = TimeSpan.FromMilliseconds(period.TotalMilliseconds / this.settings.TravelSpeedFactor);
        }

        public void CalculateTransfer(
            ILocation origin,
            ILocation destination,
            out double deltaV,
            out TimeSpan period)
        {
            this.CalculateLaunchOrLanding(
                origin, out var originOrbit, out var launchDeltaV, out var launchPeriod);

            this.CalculateLaunchOrLanding(
                destination, out var destinationOrbit, out var landingDeltaV, out var landingPeriod);

            this.CalculateTransferOrbit(
                originOrbit, 
                destinationOrbit, 
                out var transferDeltaV, 
                out var transferPeriod);

            deltaV = launchDeltaV + transferDeltaV + landingDeltaV;
            period = launchPeriod + transferPeriod + landingPeriod;
        }

        public void CalculateLaunchOrLanding(
            ILocation origin,
            out OrbitalLocation orbit,
            out double deltaV,
            out TimeSpan period)
        {
            switch (origin)
            {
                case CelestialBody celestialBody:
                    orbit = celestialBody.System.LowOrbit;
                    deltaV = OrbitMechanics.GetOrbitVelocity(celestialBody.GravitationalParameter, celestialBody.System.LowOrbit.Orbit) - celestialBody.SurfaceRotation;
                    // TODO: Move to settings
                    period = TimeSpan.FromHours(1);
                    break;

                case OrbitalLocation orbitalLocation:
                    orbit = orbitalLocation;
                    deltaV = 0;
                    period = TimeSpan.Zero;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(origin));
            }
        }

        public void CalculateTransferOrbit(
            OrbitalLocation origin,
            OrbitalLocation destination,
            out double deltaV,
            out TimeSpan period)
        {
            if (origin == destination)
            {
                deltaV = 0;
                period = TimeSpan.Zero;
                return;
            }

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

        /// <summary>
        /// E.g: From a planet to an orbiting moon
        /// or from a star to the moon of an orbiting planet.
        /// </summary>
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
                new Satellite(arrivalOrbit, plan.DestinationSystem.CentralBody.GravitationalParameter),
                arrivalOrbit);

            return new Transfer(transfer.InsertionDeltaVelocity, arrivalEscapeDeltaV + transfer.CaptureDeltaVelocity, transfer.Period);
        }

        /// <summary>
        /// E.g: From a moon to the planet
        /// or from a moon of a planet to the orbited star.
        /// </summary>
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
                new Satellite(departureOrbit, plan.DestinationSystem.CentralBody.GravitationalParameter),
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
                new Satellite(originSystem.Orbit, originSystem.CentralBody.GravitationalParameter),
                departureOrbit,
                new Satellite(destinationSystem.Orbit, destinationSystem.CentralBody.GravitationalParameter),
                arrivalOrbit);

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
                var currentEscapeDeltaV = OrbitMechanics.GetEscapeDeltaVelocity(currentBody.GravitationalParameter, currentOrbit);

                totalEscapeVelocity += currentEscapeDeltaV;

                currentOrbit = currentSystem.Orbit;
                totalOrbit += currentSystem.Orbit;
            }

            deltaV = totalEscapeVelocity;
            orbit = totalOrbit;
        }
    }
}
