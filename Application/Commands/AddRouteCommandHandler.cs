namespace SpaceLogistic.Application.Commands
{
    using System.Linq;

    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.ShipRoutes;

    public sealed class AddRouteCommandHandler : CommandHandlerBase<AddRouteCommand>
    {
        private readonly IGameProvider gameProvider;

        public AddRouteCommandHandler(IGameProvider gameProvider)
        {
            this.gameProvider = gameProvider;
        }

        public override bool CanExecute(AddRouteCommand command)
        {
            return true;
        }

        public override void Execute(AddRouteCommand command)
        {
            this.gameProvider.Get().AddRoute(new Route("Unnamed", Enumerable.Empty<RouteStop>()));
        }
    }
}