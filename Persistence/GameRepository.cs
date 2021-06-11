namespace SpaceLogistic.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Persistence.Model;

    public class GameRepository : IGameRepository
    {
        private readonly GameConverter gameConverter;

        private readonly GameDataConverter gameDataConverter;
        
        private readonly DirectoryInfo saveDirectory = new DirectoryInfo(@"..\..\..\saves");

        private readonly string saveFileEnding = ".save.json";

        public GameRepository(GameConverter gameConverter, GameDataConverter gameDataConverter)
        {
            this.gameConverter = gameConverter;
            this.gameDataConverter = gameDataConverter;
        }
        
        public IReadOnlyCollection<SaveGame> GetSaveGames()
        {
            if (!this.saveDirectory.Exists)
            {
                return new SaveGame[0];
            }
            
            return this.saveDirectory
                .GetFiles()
                .Where(f => f.Name.EndsWith(this.saveFileEnding, StringComparison.InvariantCulture))
                .Select(f => new SaveGame(Guid.Parse(f.Name.Substring(0, f.Name.Length - this.saveFileEnding.Length))))
                .ToList();
        }

        public Game GetGame(SaveGame save)
        {
            var saveFilePath = Path.Combine(this.saveDirectory.FullName, save.Id + this.saveFileEnding);
            var saveGameJson = File.ReadAllText(saveFilePath);
            var gameData = JsonConvert.DeserializeObject<GameData>(saveGameJson);
            return this.gameConverter.Convert(gameData);
        }

        public void SaveGame(SaveGame save, Game game)
        {
            this.saveDirectory.Create();
            var saveFilePath = Path.Combine(this.saveDirectory.FullName, save.Id + this.saveFileEnding);
            var gameData = this.gameDataConverter.Convert(game);
            File.WriteAllText(saveFilePath, JsonConvert.SerializeObject(gameData));
        }
    }
}
