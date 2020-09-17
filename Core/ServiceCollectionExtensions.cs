namespace SpaceLogistic.Core
{
    using Microsoft.Extensions.DependencyInjection;

    using SpaceLogistic.Core.Model.Items;
    using SpaceLogistic.Core.Model.Resources;
    using SpaceLogistic.Core.Model.Ships;
    using SpaceLogistic.Core.Model.Structures;
    using SpaceLogistic.Core.Services;
    using SpaceLogistic.Core.Services.WorldGeneration;
    using SpaceLogistic.Core.Services.WorldGeneration.Import;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddServices()
                .AddSingletonModels();
        }

        private static IServiceCollection AddSingletonModels(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ResourceTypes>()
                .AddSingleton<ItemTypes>()
                .AddSingleton<StructureTypes>()
                .AddSingleton<ShipTypes>();
        }

        private static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ITransferCalculator, TransferCalculator>()
                .AddSingleton<IStarSystemImporter, StarSystemImporter>()
                .AddSingleton<IWorldGenerator, WorldGenerator>()
                .AddSingleton<IGameUpdateService, GameUpdateService>();
        }
    }
}
