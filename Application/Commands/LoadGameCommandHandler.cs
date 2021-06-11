namespace SpaceLogistic.Application.Commands
{
    using System;
    using System.Linq;
    using SpaceLogistic.Application.CommandPattern;
    using SpaceLogistic.Persistence;

    public sealed class LoadGameCommandHandler : CommandHandlerBase<LoadGameCommand>
    {
        private readonly IGameRepository gameRepository;
        
        private readonly IGameApplicationState gameApplicationState;

        public LoadGameCommandHandler(IGameRepository gameRepository, IGameApplicationState gameApplicationState)
        {
            this.gameRepository = gameRepository;
            this.gameApplicationState = gameApplicationState;
        }
        
        public override bool CanExecute(LoadGameCommand command)
        {
            return true;
        }

        public override void Execute(LoadGameCommand command)
        {
            var save = this.gameRepository.GetSaveGames().FirstOrDefault();

            if (save == null)
            {
                throw new InvalidOperationException("Can not execute command.");
            }

            this.gameApplicationState.Set(this.gameRepository.GetGame(save));
        }
    }
}