namespace SpaceLogistic.Application
{
    using SpaceLogistic.Core.Model;

    public interface IGameApplicationState : IGameProvider
    {
        void Set(Game game);
    }
}