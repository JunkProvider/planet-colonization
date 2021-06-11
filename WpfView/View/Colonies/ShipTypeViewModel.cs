namespace SpaceLogistic.WpfView.View.Colonies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.Items;
    using SpaceLogistic.Core.Model.Ships;

    public sealed class ShipTypeViewModel : ViewModelBase, IIdentity
    {
        private Guid id;

        private string name;

        private string description;

        private IReadOnlyCollection<Item> constructionMaterials;

        public Guid Id
        {
            get => this.id;
            private set => this.SetProperty(ref this.id, value);
        }

        public string Name
        {
            get => this.name;
            private set => this.SetProperty(ref this.name, value);
        }

        public string Description
        {
            get => this.description;
            private set => this.SetProperty(ref this.description, value);
        }

        public IReadOnlyCollection<Item> ConstructionMaterials
        {
            get => this.constructionMaterials;
            private set => this.SetProperty(ref this.constructionMaterials, value);
        }

        public void Update(ShipType model)
        {
            this.Id = model.Id;
            this.Name = model.Name;
            this.Description = model.Description;
            this.ConstructionMaterials = model.ConstructionMaterials.ToList();
        }
    }
}