namespace SpaceLogistic.Core.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using SpaceLogistic.Core.Model.Celestials;

    public sealed class TransferPlan
    {
        public static TransferPlan Create(OrbitalLocation origin, OrbitalLocation destination)
        {
            var primarySystem = CelestialSystem.GetFirstCommonAncestorOrSelf(origin.System, destination.System);

            // E.g: From Earth to Luna
            if (origin.System == primarySystem)
            {
                return new TransferPlan(
                    primarySystem,
                    primarySystem,
                    new List<CelestialSystem>(), 
                    origin.Orbit,
                    destination.System.GetSelfAndAncestors().First(s => s.Parent == primarySystem),
                    destination.System.GetSelfAndAncestors().TakeWhile(s => s.Parent != primarySystem).ToList(),
                    destination.Orbit);
            }

            // E.g: From Luna to Earth
            if (destination.System == primarySystem)
            {
                return new TransferPlan(
                    primarySystem,
                    origin.System.GetSelfAndAncestors().First(s => s.Parent == primarySystem),
                    origin.System.GetSelfAndAncestors().TakeWhile(s => s.Parent != primarySystem).ToList(),
                    origin.Orbit,
                    primarySystem,
                    new List<CelestialSystem>(),
                    destination.Orbit);
            }

            // E.g: From Earth to Mars or Luna to Mars ...
            var originSystem = origin.System.GetSelfAndAncestors().First(s => s.Parent == primarySystem);
            var destinationSystem = destination.System.GetSelfAndAncestors().First(s => s.Parent == primarySystem);
            
            return new TransferPlan(
                primarySystem,
                originSystem,
                origin.System.GetSelfAndAncestors().TakeWhile(s => s != originSystem).ToList(),
                origin.Orbit,
                destinationSystem,
                destination.System.GetSelfAndAncestors().TakeWhile(s => s != destinationSystem).ToList(),
                destination.Orbit);
        }

        public TransferPlan(CelestialSystem primarySystem, CelestialSystem originSystem, IReadOnlyCollection<CelestialSystem> originSubsystems, double originOrbit, CelestialSystem destinationSystem, IReadOnlyCollection<CelestialSystem> destinationSubsystems, double destinationOrbit)
        {
            this.PrimarySystem = primarySystem;
            this.OriginSystem = originSystem;
            this.OriginSubsystems = originSubsystems;
            this.DestinationSystem = destinationSystem;
            this.DestinationSubsystems = destinationSubsystems;
            this.OriginOrbit = originOrbit;
            this.DestinationOrbit = destinationOrbit;
        }

        public CelestialSystem PrimarySystem { get; }

        public CelestialSystem OriginSystem { get; }

        public IReadOnlyCollection<CelestialSystem> OriginSubsystems { get; }

        public double OriginOrbit { get; }

        public CelestialSystem DestinationSystem { get; }

        public IReadOnlyCollection<CelestialSystem> DestinationSubsystems { get; }

        public double DestinationOrbit { get; }
    }
}

