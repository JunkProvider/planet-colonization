namespace SpaceLogistic.WpfView.View
{
    using SpaceLogistic.Core.Model;

    public interface IPageViewModel
    {
        string Title { get; }

        void Update(Game game);
    }
}
