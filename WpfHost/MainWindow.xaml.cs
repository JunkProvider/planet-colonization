namespace SpaceLogistic.WpfHost
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading;
    using System.Windows;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using SpaceLogistic.Application;
    using SpaceLogistic.Core;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Services;
    using SpaceLogistic.Core.Services.WorldGeneration;
    using SpaceLogistic.Core.Services.WorldGeneration.Import;
    using SpaceLogistic.Persistence;
    using SpaceLogistic.WpfView;
    using SpaceLogistic.WpfView.View;
    using SpaceLogistic.WpfView.ViewModel;

    public partial class MainWindow : Window
    {
        private readonly IServiceProvider serviceProvider;
        
        public MainWindow()
        {
            MainWindowEvents.Setup(this);

            var serviceCollection = new ServiceCollection()
                .AddCore()
                .AddApplication()
                .AddPersistence()
                .AddView()

                // Initialization
                .AddSingleton<GameFactory>()
                .AddSingleton(new WorldSettings())
                
                .AddSingleton(this.Dispatcher)
                .AddSingleton<IGameLoop, GameLoop>();

            this.serviceProvider = serviceCollection.BuildServiceProvider();
            
            LoadGame(this.serviceProvider);
            
            var hostedServices = this.serviceProvider.GetRequiredService<IEnumerable<IHostedService>>();

            foreach (var hostedService in hostedServices)
            {
                hostedService.StartAsync(CancellationToken.None).GetAwaiter().GetResult();
            }
            
            this.DataContext = this.serviceProvider.GetRequiredService<GameViewModel>();
            this.InitializeComponent();

            var gameLoop = this.serviceProvider.GetRequiredService<IGameLoop>();
            gameLoop.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            var gameApplicationState = serviceProvider.GetRequiredService<IGameApplicationState>();
            var gameRepository = serviceProvider.GetRequiredService<IGameRepository>();
            gameRepository.SaveGame(
                gameRepository.GetSaveGames().FirstOrDefault() ?? new SaveGame(Guid.NewGuid()),
                gameApplicationState.Get());
            
            base.OnClosing(e);
        }

        private static void LoadGame(IServiceProvider serviceProvider)
        {
            var gameApplicationState = serviceProvider.GetRequiredService<IGameApplicationState>();
            var gameRepository = serviceProvider.GetRequiredService<IGameRepository>();

            var save = gameRepository.GetSaveGames().FirstOrDefault();

            if (save != null)
            {
                gameApplicationState.Set(gameRepository.GetGame(save));
            }
            else
            {
                var startSystemImporter = serviceProvider.GetRequiredService<IStarSystemImporter>();
                var worldGenerator = serviceProvider.GetRequiredService<IWorldGenerator>();
                var gameFactory = serviceProvider.GetRequiredService<GameFactory>();
                var startSystem = startSystemImporter.Import(@"F:\Stuff\Data\solar-system.json");
                worldGenerator.Modify(startSystem);
                var game = gameFactory.Create(startSystem.Build());
            
                gameApplicationState.Set(game);   
            }
        }
    }
}
