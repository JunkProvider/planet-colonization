﻿namespace SpaceLogistic.WpfHost.WpfViewHosting
{
    using Microsoft.Extensions.DependencyInjection;

    using SpaceLogistic.WpfHost.ApplicationHosting;
    using SpaceLogistic.WpfView.Commands;
    using SpaceLogistic.WpfView.ViewModel;
    using SpaceLogistic.WpfView.ViewModel.Colonies;

    using ShipViewModel = SpaceLogistic.WpfView.ViewModel.Colonies.ShipViewModel;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddView(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingletonViewModels()
                .AddViewModelFactories()
                .AddCommandHandlers();
        }

        private static IServiceCollection AddSingletonViewModels(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<GameViewModel>()
                .AddSingleton<ColonyPageViewModel>();
        }

        private static IServiceCollection AddViewModelFactories(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddViewModelFactory<ColonyViewModel>()
                .AddViewModelFactory<StructureViewModel>()
                .AddViewModelFactory<ShipViewModel>()
                .AddViewModelFactory<StructureTypeViewModel>()
                .AddViewModelFactory<ShipTypeViewModel>()
                .AddViewModelFactory<AddStructureOverlayViewModel>()
                .AddViewModelFactory<AddShipOverlayViewModel>();
        }

        private static IServiceCollection AddViewModelFactory<TViewModel>(this IServiceCollection serviceCollection)
            where TViewModel : class
        {
            return serviceCollection
                .AddTransient<TViewModel>()
                .AddSingleton<IViewModelFactory<TViewModel>, ServiceProviderViewModelFactory<TViewModel>>();
        }

        private static IServiceCollection AddCommandHandlers(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddCommandHandler<CloseOverlayCommandHandler>()
                .AddCommandHandler<SwitchToNextColonyCommandHandler>()
                .AddCommandHandler<OpenAddStructureOverlayCommandHandler>()
                .AddCommandHandler<OpenAddShipOverlayCommandHandler>();
        }
    }
}
