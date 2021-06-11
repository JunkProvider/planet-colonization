namespace SpaceLogistic.WpfView.View.Routes
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Windows.Input;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.Items;
    using SpaceLogistic.Core.Model.ShipRoutes;
    using SpaceLogistic.WpfView.Utility;

    public sealed class LoadInstructionViewModel : ViewModelBase, IIdentity
    {
        private readonly ItemTransferInstruction loadInstruction;

        public LoadInstructionViewModel(ItemTypes itemTypes, ItemTransferInstruction loadInstruction, bool isLoadInstruction, Action<Guid> deleteAction)
        {
            this.IsLoadInstruction = isLoadInstruction;
            this.loadInstruction = loadInstruction;
            this.AvailableItemTypes = new ObservableCollection<ItemType>(itemTypes.GetAll());
            this.EditCommand = new DelegateCommand<ItemTransferInstruction>(this.Edit);
            this.DeleteCommand = new DelegateCommand(() => deleteAction(this.Id));
        }
        
        public Guid Id => this.loadInstruction.Id;

        public ItemTransferInstruction Model => this.loadInstruction;

        public bool IsLoadInstruction { get; }

        public string ItemName => this.loadInstruction.ItemType.Name;
        
        public string AmountString
        {
            get => this.loadInstruction.Amount.ToString(CultureInfo.InvariantCulture);
            set
            {
                this.SetAmount(value);
                this.OnPropertyChanged();
            }
        }
        
        public ItemType SelectedItemType    
        {
            get => this.loadInstruction.ItemType;
            set
            {
                this.SetItemType(value);
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.ItemName));
            }
        }

        public ObservableCollection<ItemType> AvailableItemTypes { get; }

        public ICommand EditCommand { get; }

        public ICommand DeleteCommand { get; }

        public void Update()
        {
            this.AmountString = this.loadInstruction.Amount.ToString(CultureInfo.InvariantCulture);
            this.SelectedItemType = this.loadInstruction.ItemType;
        }

        public override string ToString()
        {
            return $"{this.loadInstruction.Amount} {this.loadInstruction.ItemType.Name}";
        }

        private void SetItemType(ItemType itemType)
        {
            this.loadInstruction.ItemType = itemType;
        }

        private void SetAmount(string amountString)
        {
            if (!int.TryParse(amountString, out var amount))
            {
                return;
            }

            this.SetAmount(amount);
        }

        private void SetAmount(int amount)
        {
            this.loadInstruction.Amount = amount;
        }

        private void Edit(ItemTransferInstruction newData)
        {
            this.loadInstruction.ItemType = newData.ItemType;
            this.loadInstruction.Amount = newData.Amount;
        }
    }
}