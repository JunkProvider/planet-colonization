namespace SpaceLogistic.WpfView.View
{
    using System.Collections;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Input;

    using SpaceLogistic.WpfView.Annotations;

    public partial class SelectButton : INotifyPropertyChanged
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyPropertyExtensions.Register<SelectButton, IEnumerable>(nameof(ItemsSource));

        public static readonly DependencyProperty ItemTemplateProperty = DependencyPropertyExtensions.Register<SelectButton, DataTemplate>(nameof(ItemTemplate));
        
        public static readonly DependencyProperty ButtonContentProperty = DependencyPropertyExtensions.Register<SelectButton, object>(nameof(ButtonContent));

        public static readonly DependencyProperty ButtonContentTemplateProperty = DependencyPropertyExtensions.Register<SelectButton, DataTemplate>(nameof(ButtonContentTemplate));

        public static readonly DependencyProperty CommandProperty = DependencyPropertyExtensions.Register<SelectButton, ICommand>(nameof(Command));

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)this.GetValue(ItemsSourceProperty);
            set => this.SetValue(ItemsSourceProperty, value);
        }

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)this.GetValue(ItemTemplateProperty);
            set => this.SetValue(ItemTemplateProperty, value);
        }

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

        public ICommand Command
        {
            get => (ICommand)this.GetValue(CommandProperty);
            set => this.SetValue(CommandProperty, value);
        }

        public object SelectedItem
        {
            get => null;
            set
            {
                if (this.Command?.CanExecute(value) ?? false)
                {
                    this.Command.Execute(value);
                }

                this.OnPropertyChanged();
            }
        }

        public SelectButton()
        {
            this.InitializeComponent();
        }

        [NotifyPropertyChangedInvocator]
        private void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value))
            {
                return;
            }

            field = value;

            this.OnPropertyChanged(propertyName);
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
