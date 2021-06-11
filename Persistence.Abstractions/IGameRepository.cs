namespace SpaceLogistic.Persistence
{
    using System.Collections.Generic;
    using SpaceLogistic.Core.Model;

    public interface IGameRepository
    {
        IReadOnlyCollection<SaveGame> GetSaveGames();

        Game GetGame(SaveGame saveGame);

        void SaveGame(SaveGame save, Game game);
    }
}
