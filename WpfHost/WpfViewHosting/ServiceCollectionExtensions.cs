namespace SpaceLogistic.WpfHost.WpfViewHosting
{
    using Microsoft.Extensions.DependencyInjection;

    using SpaceLogistic.WpfHost.ApplicationHosting;
    using SpaceLogistic.WpfView.Commands;
    using SpaceLogistic.WpfView.ViewModel;

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
                .AddViewModelFactory<StructureViewModel>();
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
                .AddCommandHandler<SwitchToNextColonyCommandHandler>();
        }
    }
}
