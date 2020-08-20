namespace SpaceLogistic.Application.Commands
{
    using System.Linq;

    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Model.ShipRoutes;

    public sealed class AddRouteCommandHandler : CommandHandlerBase<AddRouteCommand>
    {
        private readonly Game game;

        public AddRouteCommandHandler(Game game)
        {
            this.game = game;
        }

        public override bool CanExecute(AddRouteCommand command)
        {
            return true;
        }

        public override void Execute(AddRouteCommand command)
        {
            game.AddRoute(new Route("Unnamed", Enumerable.Empty<RouteStop>()));
        }
    }
}