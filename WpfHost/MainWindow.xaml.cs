namespace SpaceLogistic.WpfHost
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Windows;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using SpaceLogistic.Core;
    using SpaceLogistic.Core.Model.Resources;
    using SpaceLogistic.Core.Services;
    using SpaceLogistic.Core.Services.WorldGeneration;
    using SpaceLogistic.Core.Services.WorldGeneration.Import;
    using SpaceLogistic.WpfHost.ApplicationHosting;
    using SpaceLogistic.WpfHost.WpfViewHosting;
    using SpaceLogistic.WpfView;
    using SpaceLogistic.WpfView.View;
    using SpaceLogistic.WpfView.ViewModel;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            MainWindowEvents.Setup(this);

            var serviceCollection = new ServiceCollection()

                .AddCore()
                .AddApplication()
                .AddView()

                // Initialization
                .AddSingleton(new WorldSettings())
                .AddSingleton(provider => provider.GetRequiredService<IStarSystemImporter>()
                    .Import(@"G:\Stuff\Data\solar-system.json"))
                .AddSingleton(provider => provider.GetRequiredService<IWorldGenerator>()
                    .Modify(provider.GetRequiredService<CelestialBodyBuilder>()).Build())
                .AddSingleton<GameFactory>()
                .AddSingleton(provider => provider.GetRequiredService<GameFactory>().Create())
                
                .AddSingleton(this.Dispatcher)
                .AddSingleton<IGameLoop, GameLoop>();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var hostedServices = serviceProvider.GetRequiredService<IEnumerable<IHostedService>>();

            foreach (var hostedService in hostedServices)
            {
                hostedService.StartAsync(CancellationToken.None).GetAwaiter().GetResult();
            }

            this.DataContext = serviceProvider.GetRequiredService<GameViewModel>();
            this.InitializeComponent();

            var gameLoop = serviceProvider.GetRequiredService<IGameLoop>();
            gameLoop.Start();
        }
    }
}
