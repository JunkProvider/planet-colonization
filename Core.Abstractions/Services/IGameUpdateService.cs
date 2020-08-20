namespace SpaceLogistic.Core.Services
{
    using System;

    using SpaceLogistic.Core.Model;

    public interface IGameUpdateService
    {
        void Startup(Game game);

        void Update(Game game, TimeSpan elapsedTime);
    }
}