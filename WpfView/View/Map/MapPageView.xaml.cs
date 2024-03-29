﻿namespace SpaceLogistic.WpfView.View.Map
{
    using System.Windows;
    using System.Windows.Input;

    public partial class MapPageView
    {
        public MapPageView()
        {
            this.InitializeComponent();
        }

        private void MapPageView_OnLoaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += this.OnWindowKeyDown;
        }

        private void OnWindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Subtract || e.Key == Key.OemMinus)
            {
                (this.DataContext as MapPageViewModel)?.DeselectSystemCommand.Execute(null);
            }
        }
    }
}
