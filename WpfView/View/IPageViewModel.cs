namespace SpaceLogistic.WpfView.ViewModel
{
    using SpaceLogistic.Core.Model;

    public interface IPageViewModel
    {
        string Title { get; }

        void Update(Game game);
    }
}
