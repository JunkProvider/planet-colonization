namespace SpaceLogistic.WpfView.View.SelectionBox
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    public partial class SelectionBox : UserControl
    {
        public static readonly DependencyProperty OptionsProperty = DependencyPropertyExtensions.RegisterEnumerable<SelectionBox, IOption>(nameof(Options));

        public static readonly DependencyProperty SelectedOptionProperty = DependencyPropertyExtensions.Register<SelectionBox, IOption>(nameof(SelectedOption));

        public IEnumerable<IOption> Options
        {
            get => this.GetValue(OptionsProperty) as IEnumerable<IOption>;
            set => this.SetValue(OptionsProperty, value);
        }

        public IOption SelectedOption
        {
            get => this.GetValue(SelectedOptionProperty) as IOption;
            set => this.SetValue(OptionsProperty, value);
        }

        public SelectionBox()
        {
            this.InitializeComponent();
        }
    }
}
