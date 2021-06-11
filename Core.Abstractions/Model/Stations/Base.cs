namespace SpaceLogistic.Core.Model.Stations
{
    using System;
    using System.Collections.Generic;
    using SpaceLogistic.Core.Model.Celestials;
    using SpaceLogistic.Core.Model.Items;
    using SpaceLogistic.Core.Model.Resources;
    using SpaceLogistic.Core.Model.Structures;

    public sealed class Base : Colony
    {
        public Base(string name)
            : base(name)
        {
        }

        public Base(
            IEnumerable<Structure> structures,
            IEnumerable<ShipConstructionProcess> shipConstructionProcesses,
            Guid id,
            string name,
            double fuelStorageCapacity,
            double storedFuel,
            FuelStorageReplenishProcess fuelStorageReplenishProcess,
            Storage warehouse) 
            : base(structures, shipConstructionProcesses, id, name, fuelStorageCapacity, storedFuel, fuelStorageReplenishProcess, warehouse)
        {
        }

        public override ResourceCollection GetAvailableResources()
        {
            return this.Location.Resources;
        }
    }
}
