namespace SpaceLogistic.WpfView.ViewModel
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
            get => id;
            private set => SetProperty(ref id, value);
        }

        public string Name
        {
            get => name;
            private set => SetProperty(ref name, value);
        }

        public string Description
        {
            get => description;
            private set => SetProperty(ref description, value);
        }

        public IReadOnlyCollection<Item> ConstructionMaterials
        {
            get => constructionMaterials;
            private set => SetProperty(ref constructionMaterials, value);
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