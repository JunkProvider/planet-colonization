namespace SpaceLogistic.WpfHost.ApplicationHosting
{
    using Microsoft.Extensions.DependencyInjection;

    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Application.Commands;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddCommandPattern()
                .AddCommandHandlers();
        }
        
        public static IServiceCollection AddCommandHandler<TCommandHandler>(this IServiceCollection serviceCollection)
            where TCommandHandler : class, ICommandHandler
        {
            return serviceCollection
                .AddSingleton<ICommandHandler, TCommandHandler>();
        }

        private static IServiceCollection AddCommandPattern(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<CommandDispatcher>()
                .AddSingletonAlias<ICommandDispatcher, CommandDispatcher>()
                .AddHostedService<CommandExecutor>();
        }

        private static IServiceCollection AddCommandHandlers(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddCommandHandler<AddRouteCommandHandler>()
                .AddCommandHandler<DeleteRouteCommandHandler>()
                .AddCommandHandler<AddStopCommandHandler>()
                .AddCommandHandler<RemoveStopCommandHandler>()
                .AddCommandHandler<AssignShipCommandHandler>()
                .AddCommandHandler<DeassignShipCommandHandler>()
                .AddCommandHandler<AddStructureCommandHandler>()
                .AddCommandHandler<RemoveStructureCommandHandler>()
                .AddCommandHandler<AddShipCommandHandler>()
                .AddCommandHandler<RenameShipCommandHandler>();
        }
    }
}
