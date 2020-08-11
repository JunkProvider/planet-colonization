namespace SpaceLogistic.WpfView.View
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    public class MainWindowEvents
    {
        private static readonly MainWindowEvents Instance = new MainWindowEvents();

        public event EventHandler<MouseEventArgs> MouseDown;

        public static void Setup(Window mainWindow)
        {
            mainWindow.MouseDown += OnMouseDown;
        }
        
        public static void RegisterOnMouseDown(EventHandler<MouseButtonEventArgs> handler)
        {
            WeakEventManager<MainWindowEvents, MouseButtonEventArgs>.AddHandler(Instance, nameof(MainWindowEvents.MouseDown), handler);
        }

        public static void DeregisterFromMouseDown(EventHandler<MouseButtonEventArgs> handler)
        {
            WeakEventManager<MainWindowEvents, MouseButtonEventArgs>.RemoveHandler(Instance, nameof(MainWindowEvents.MouseDown), handler);
        }

        public static void RaiseMouseDown(MouseButtonEventArgs args)
        {
            Instance.MouseDown?.Invoke(Instance, args);
        }

        private static void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            RaiseMouseDown(e);
        }
    }
}
