namespace SpaceLogistic.Persistence.Model.Structures
{
    using System;
    using System.Collections.Generic;

    using SpaceLogistic.Core.Model.Items;
    using SpaceLogistic.Core.Model.Resources;

    public sealed class StructureData
    {
        public StructureData(Guid id, Guid structureType, ProductionProcessData productionProcess)
        {
            this.Id = id;
            this.StructureType = structureType;
            this.ProductionProcess = productionProcess;
        }
        
        public Guid Id { get; }

        public Guid StructureType { get; }

        public ProductionProcessData ProductionProcess { get; set; }
    }
}