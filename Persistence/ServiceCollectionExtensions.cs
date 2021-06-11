namespace SpaceLogistic.Persistence
{
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<GameConverter>()
                .AddSingleton<GameDataConverter>()
                .AddSingleton<IGameRepository, GameRepository>();
        }
    }
}