namespace SpaceLogistic.WpfHost
{
    using System.Windows;

    using Microsoft.Extensions.DependencyInjection;

    using SpaceLogistic.Core.Commands;
    using SpaceLogistic.Core.Model.Resources;
    using SpaceLogistic.Core.Services;
    using SpaceLogistic.Core.Services.WorldGeneration;
    using SpaceLogistic.Core.Services.WorldGeneration.Import;
    using SpaceLogistic.WpfView;
    using SpaceLogistic.WpfView.View;
    using SpaceLogistic.WpfView.ViewModel;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            MainWindowEvents.Setup(this);

            var serviceCollection = new ServiceCollection()
                .AddSingleton<ITransferCalculator, TransferCalculator>()
                .AddSingleton<IGameUpdateService, GameUpdateService>()
                .AddCommandPattern()
                .AddCommandHandler<AddRouteCommandHandler>()
                .AddCommandHandler<DeleteRouteCommandHandler>()
                .AddCommandHandler<AddStopCommandHandler>()
                .AddCommandHandler<RemoveStopCommandHandler>()
                .AddCommandHandler<AssignShipCommandHandler>()
                .AddCommandHandler<DeassignShipCommandHandler>()

                // Initialization
                .AddSingleton(new WorldSettings())
                .AddSingleton<ResourceTypes>()
                .AddSingleton<StarSystemImporter>()
                .AddSingleton<WorldGenerator>()
                .AddSingleton(provider => provider.GetRequiredService<StarSystemImporter>()
                    .Import(@"G:\Stuff\Data\solar-system.json"))
                .AddSingleton(provider => provider.GetRequiredService<WorldGenerator>()
                    .Modify(provider.GetRequiredService<CelestialBodyBuilder>()).Build())
                .AddSingleton<GameFactory>()
                .AddSingleton(provider => provider.GetRequiredService<GameFactory>().Create())
                
                .AddSingleton<GameViewModel>()

                .AddSingleton(this.Dispatcher)
                .AddSingleton<IGameLoop, GameLoop>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var viewModel = serviceProvider.GetRequiredService<GameViewModel>();
            var gameLoop = serviceProvider.GetRequiredService<IGameLoop>();

            this.DataContext = viewModel;
            this.InitializeComponent();
            gameLoop.Start();
        }
    }
}
