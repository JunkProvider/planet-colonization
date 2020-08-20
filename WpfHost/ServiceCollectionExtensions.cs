namespace SpaceLogistic.WpfHost
{
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSingletonAlias<TAlias, TService>(this IServiceCollection serviceCollection)
            where TAlias : class
            where TService : class, TAlias
        {
            return serviceCollection
                .AddSingleton<TAlias>(provider => provider.GetRequiredService<TService>());
        }
    }
}
