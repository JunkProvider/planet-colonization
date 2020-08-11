namespace SpaceLogistic.WpfHost
{
    using Microsoft.Extensions.DependencyInjection;

    using SpaceLogistic.Core.CommandPattern;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommandPattern(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ICommandDispatcher, CommandDispatcher>();
        }


        public static IServiceCollection AddCommandHandler<TCommandHandler>(this IServiceCollection serviceCollection)
            where TCommandHandler : class, ICommandHandler
        {
            return serviceCollection
                .AddSingleton<ICommandHandler, TCommandHandler>();
        }
    }
}
