namespace SpaceLogistic.WpfView.View.Controls
{
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;

    using SpaceLogistic.WpfView.Utility;

    public partial class ObjectListItem
    {
        public static readonly DependencyProperty TextProperty = DependencyPropertyExtensions.Register<ObjectListItem, string>(nameof(Text));

        public static readonly DependencyProperty IsRenameEnabledProperty = DependencyPropertyExtensions.Register<ObjectListItem, bool>(nameof(IsRenameEnabled));

        public static readonly DependencyProperty IsSelectEnabledProperty = DependencyPropertyExtensions.Register<ObjectListItem, bool>(nameof(IsSelectEnabled));

        public static readonly DependencyProperty IsRemoveEnabledProperty = DependencyPropertyExtensions.Register<ObjectListItem, bool>(nameof(IsRemoveEnabled));

        public static readonly DependencyProperty RemoveCommandProperty = DependencyPropertyExtensions.Register<ObjectListItem, ICommand>(nameof(RemoveCommand));

        public ObjectListItem()
        {
            this.InitializeComponent();
        }

        public string Text
        {
            get => (string)this.GetValue(TextProperty);
            set => this.SetValue(TextProperty, value);
        }

        public bool IsRenameEnabled
        {
            get => (bool)this.GetValue(IsRenameEnabledProperty);
            set => this.SetValue(IsRenameEnabledProperty, value);
        }

        public bool IsSelectEnabled
        {
            get => (bool)this.GetValue(IsSelectEnabledProperty);
            set => this.SetValue(IsSelectEnabledProperty, value);
        }

        public bool IsRemoveEnabled
        {
            get => (bool)this.GetValue(IsRemoveEnabledProperty);
            set => this.SetValue(IsRemoveEnabledProperty, value);
        }

        public ICommand RemoveCommand
        {
            get => (ICommand)this.GetValue(RemoveCommandProperty);
            set => this.SetValue(RemoveCommandProperty, value);
        }

        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    this.GetTextBoxTextBinding()?.UpdateSource();
                    Keyboard.ClearFocus();
                    break;
                case Key.Escape:
                    this.GetTextBoxTextBinding()?.UpdateTarget();
                    Keyboard.ClearFocus();
                    break;
            }
        }

        private BindingExpression GetTextBoxTextBinding()
        {
            return this.TextBox.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty);
        }
    }
}
