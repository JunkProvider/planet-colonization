namespace SpaceLogistic.Application
{
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IGameApplicationState, GameApplicationState>()
                .AddSingleton<IGameProvider>(provider => provider.GetRequiredService<IGameApplicationState>());
        }
    }
}