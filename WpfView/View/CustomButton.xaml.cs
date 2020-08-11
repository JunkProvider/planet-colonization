namespace SpaceLogistic.WpfView.View
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Input;

    using SpaceLogistic.WpfView.Annotations;

    public partial class CustomButton : INotifyPropertyChanged
    {
        public static readonly DependencyProperty ButtonContentProperty = DependencyPropertyExtensions.Register<CustomButton, object>(nameof(ButtonContent));

        public static readonly DependencyProperty ButtonContentTemplateProperty = DependencyPropertyExtensions.Register<CustomButton, DataTemplate>(nameof(ButtonContentTemplate));

        public static readonly DependencyProperty BorderStyleProperty = DependencyPropertyExtensions.Register<CustomButton, Style>(nameof(BorderStyle), OnBorderStylePropertyChanged);
        
        public static readonly DependencyProperty BorderActiveStyleProperty = DependencyPropertyExtensions.Register<CustomButton, Style>(nameof(BorderActiveStyle), OnBorderStylePropertyChanged);

        public static readonly DependencyProperty CommandProperty = DependencyPropertyExtensions.Register<CustomButton, ICommand>(nameof(Command));

        public static readonly DependencyProperty CommandParameterProperty = DependencyPropertyExtensions.Register<CustomButton, object>(nameof(CommandParameter));

        private static void OnBorderStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CustomButton)d).OnBorderStyleChanged();
        }

        private bool isPressed;

        private bool isActive;

        private Style internalBorderStyle;

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

        public Style BorderStyle
        {
            get => (Style)this.GetValue(BorderStyleProperty);
            set => this.SetValue(BorderStyleProperty, value);
        }

        public Style BorderActiveStyle
        {
            get => (Style)this.GetValue(BorderActiveStyleProperty);
            set => this.SetValue(BorderActiveStyleProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)this.GetValue(CommandProperty);
            set => this.SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => this.GetValue(CommandParameterProperty);
            set => this.SetValue(CommandParameterProperty, value);
        }

        public bool IsPressed
        {
            get => this.isPressed;
            private set => this.SetProperty(ref this.isPressed, value);
        }

        public bool IsActive
        {
            get => this.isActive;
            private set => this.SetProperty(ref this.isActive, value);
        }

        public Style InternalBorderStyle
        {
            get => this.internalBorderStyle;
            private set => this.SetProperty(ref this.internalBorderStyle, value);
        }

        public CustomButton()
        {
            this.InitializeComponent();
        }
        
        private void OnBorderStyleChanged()
        {
            this.ApplyBorderStyle();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.IsPressed = true;
        }

        private void UIElement_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.IsPressed)
            {
                this.OnClicked();
            }

            this.IsPressed = false;
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (this.CanBePressed())
            {
                this.IsActive = true;
                this.ApplyBorderStyle();
            }
        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
            this.IsActive = false;
            this.IsPressed = false;
            this.ApplyBorderStyle();
        }

        private void OnClicked()
        {
            if (this.Command != null && this.Command.CanExecute(this.CommandParameter))
            {
                this.Command.Execute(this.CommandParameter);
            }
        }

        private bool CanBePressed()
        {
            return this.Command?.CanExecute(this.CommandParameter) ?? true;
        }

        private void ApplyBorderStyle()
        {
            this.InternalBorderStyle = this.IsActive && this.BorderActiveStyle != null
                ? this.BorderActiveStyle
                : this.BorderStyle;
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
