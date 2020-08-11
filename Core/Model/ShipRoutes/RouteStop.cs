namespace SpaceLogistic.Core.Model.ShipRoutes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Utility;

    public sealed class RouteStop : IIdentity
    {
        private readonly List<LoadInstruction> loadInstructions;

        private readonly List<UnloadInstruction> unloadInstructions;

        public RouteStop(OrbitalLocation location, RefuelBehavior refuelBehavior)
            : this(location, refuelBehavior, Enumerable.Empty<LoadInstruction>(), Enumerable.Empty<UnloadInstruction>())
        {
        }

        public RouteStop(OrbitalLocation location, RefuelBehavior refuelBehavior, IEnumerable<LoadInstruction> loadInstructions, IEnumerable<UnloadInstruction> unloadInstructions)
        {
            this.Location = location;
            this.RefuelBehavior = refuelBehavior;
            this.loadInstructions = loadInstructions.ToList();
            this.unloadInstructions = unloadInstructions.ToList();
        }

        public Guid Id { get; } = Guid.NewGuid();

        public OrbitalLocation Location { get; }

        public RefuelBehavior RefuelBehavior { get; set; }

        public IReadOnlyCollection<LoadInstruction> LoadInstructions => this.loadInstructions;

        public IReadOnlyCollection<UnloadInstruction> UnloadInstructions => this.unloadInstructions;

        public override string ToString()
        {
            return this.Location.Name;
        }

        public void AddLoadInstruction(LoadInstruction instruction)
        {
            this.loadInstructions.Add(instruction);
        }

        public void AddUnloadInstruction(UnloadInstruction instruction)
        {
            this.unloadInstructions.Add(instruction);
        }

        public void RemoveLoadInstruction(Guid instructionId)
        {
            this.loadInstructions.RemoveWhere(i => i.Id == instructionId);
        }

        public void RemoveUnloadInstruction(Guid instructionId)
        {
            this.unloadInstructions.RemoveWhere(i => i.Id == instructionId);
        }
    }
}