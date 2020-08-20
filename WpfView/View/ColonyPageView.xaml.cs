namespace SpaceLogistic.WpfView.View
{
    using System.Windows;
    using System.Windows.Input;

    using SpaceLogistic.WpfView.ViewModel;

    public partial class ColonyPageView
    {
        public ColonyPageView()
        {
            this.InitializeComponent();
        }

        private void ColonyPageView_OnLoaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += this.OnWindowKeyDown;
        }

        private void OnWindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Add || e.Key == Key.OemPlus)
            {
                (this.DataContext as ColonyPageViewModel)?.MoveToNextColonyCommand.Execute(null);
            }
        }
    }
}
