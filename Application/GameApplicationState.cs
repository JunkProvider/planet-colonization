namespace SpaceLogistic.Application
{
    using System;
    using SpaceLogistic.Core.Model;

    public sealed class GameApplicationState : IGameApplicationState
    {
        private Game game;
        
        public Game Get()
        {
            return this.game ?? throw new InvalidOperationException();
        }

        public void Set(Game game)
        {
            this.game = game;
        }
    }
}