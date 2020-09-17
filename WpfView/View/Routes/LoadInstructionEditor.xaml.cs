namespace SpaceLogistic.WpfView.View
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;

    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.ShipRoutes;
    using SpaceLogistic.WpfView.Annotations;
    using SpaceLogistic.WpfView.Utility;

    public partial class LoadInstructionEditor : INotifyPropertyChanged
    {
        public static readonly DependencyProperty ButtonContentProperty = DependencyPropertyExtensions.Register<LoadInstructionEditor, object>(nameof(ButtonContent));

        public static readonly DependencyProperty ButtonContentTemplateProperty = DependencyPropertyExtensions.Register<LoadInstructionEditor, DataTemplate>(nameof(ButtonContentTemplate));

        public static readonly DependencyProperty IsOpenProperty = DependencyPropertyExtensions.Register<LoadInstructionEditor, bool>(nameof(IsOpen), OnIsOpenPropertyChanged);

        public static readonly DependencyProperty UpdateCommandProperty = DependencyPropertyExtensions.Register<LoadInstructionEditor, ICommand>(nameof(UpdateCommand));

        public static readonly DependencyProperty DeleteCommandProperty = DependencyPropertyExtensions.Register<LoadInstructionEditor, ICommand>(nameof(DeleteCommand));

        public static readonly DependencyProperty AvailableItemTypesProperty = DependencyPropertyExtensions.RegisterEnumerable<LoadInstructionEditor, ItemType>(nameof(AvailableItemTypes));

        public static readonly DependencyProperty LoadInstructionProperty = DependencyPropertyExtensions.Register<LoadInstructionEditor, ItemTransferInstruction>(nameof(LoadInstruction), OnLoadInstructionPropertyChanged);

        private string amount;

        private ItemType itemType;

        public LoadInstructionEditor()
        {
            this.InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public object ButtonContent
        {
            get => this.GetValue(ButtonContentProperty);
            set => this.SetValue(ButtonContentProperty, value);
        }

        public DataTemplate ButtonContentTemplate
        {
            get => (DataTemplate)this.GetValue(ButtonContentTemplateProperty);
            set => this.SetValue(ButtonContentTemplateProperty, value);
        }

        public ICommand UpdateCommand
        {
            get => (ICommand)this.GetValue(UpdateCommandProperty);
            set => this.SetValue(UpdateCommandProperty, value);
        }

        public ICommand DeleteCommand
        {
            get => (ICommand)this.GetValue(DeleteCommandProperty);
            set => this.SetValue(DeleteCommandProperty, value);
        }

        public ItemTransferInstruction LoadInstruction
        {
            get => (ItemTransferInstruction)this.GetValue(LoadInstructionProperty);
            set => this.SetValue(LoadInstructionProperty, value);
        }

        public IEnumerable<ItemType> AvailableItemTypes
        {
            get => (IEnumerable<ItemType>)this.GetValue(AvailableItemTypesProperty);
            set => this.SetValue(AvailableItemTypesProperty, value);
        }

        public string Amount    
        {
            get => this.amount;
            set
            {
                if (value == this.amount)
                {
                    return;
                }

                this.amount = value;
                this.RaisePropertyChanged();
            }
        }

        public ItemType ItemType
        {
            get => this.itemType;
            set
            {
                if (value == this.itemType)
                {
                    return;
                }

                this.itemType = value;
                this.RaisePropertyChanged();
            }
        }
        
        public bool IsOpen
        {
            get => (bool)this.GetValue(IsOpenProperty);
            set => this.SetValue(IsOpenProperty, value);
        }

        private static void OnIsOpenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LoadInstructionEditor)d).OnIsOpenChanged((bool)e.NewValue);
        }
        
        private static void OnLoadInstructionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LoadInstructionEditor)d).OnLoadInstructionChanged(e.NewValue as ItemTransferInstruction);
        }

        private void OnIsOpenChanged(bool isOpen)
        {
            if (isOpen)
            {
                this.OnEditorOpened();
            }
            else
            {
                this.AcceptInput();
            }
        }

        private void OnEditorOpened()
        {
            Dispatcher.BeginInvoke(
                DispatcherPriority.ContextIdle,
                new Action(() =>
                        {
                            this.AmountInput.Focus();
                            this.AmountInput.SelectionStart = 0;
                            this.AmountInput.SelectionLength = this.AmountInput.Text.Length;
                        }));
        }

        private void AmountInput_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.IsOpen = false;
            }
        }

        private void ItemTypeInput_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.IsOpen = false;
            }
        }

        private void OnLoadInstructionChanged(ItemTransferInstruction loadInstruction)
        {
            this.Amount = loadInstruction.Amount.ToString(CultureInfo.InvariantCulture);
            this.ItemType = loadInstruction.ItemType;
        }

        private void AcceptInput()
        {
            if (this.UpdateCommand == null)
            {
                // Nothing to to with the accepted input.
                return;
            }
            
            if (this.ItemType == null || !int.TryParse(this.Amount, out var parsedAmount) || parsedAmount < 0)
            {
                // Invalid or missing input.
                return;
            }
            
            var commandParameter = new LoadInstruction(this.ItemType, parsedAmount);

            if (this.UpdateCommand.CanExecute(commandParameter))
            {
                this.UpdateCommand.Execute(commandParameter);
            }
        }

        [NotifyPropertyChangedInvocator]
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ToggleButton_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.IsOpen)
            {
                return;
            }

            if (this.DeleteCommand == null)
            {
                return;
            }

            if (this.DeleteCommand.CanExecute(null))
            {
                this.DeleteCommand.Execute(null);
            }
        }
    }
}
