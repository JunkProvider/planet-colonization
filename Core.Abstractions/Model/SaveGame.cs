using System;

namespace SpaceLogistic.Core.Model
{
    public sealed class SaveGame
    {
        public SaveGame(Guid id)
        {
            Id = id;
        }
            
        public Guid Id { get; }
    }
}
