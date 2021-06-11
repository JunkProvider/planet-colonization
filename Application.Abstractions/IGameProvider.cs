namespace SpaceLogistic.Application
{
    using System.Linq;
    using SpaceLogistic.Core.Model;

    public interface IGameProvider
    {
        Game Get();
    }
}