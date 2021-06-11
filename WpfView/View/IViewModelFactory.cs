namespace SpaceLogistic.WpfView.View
{
    public interface IViewModelFactory<out TViewModel>
        where TViewModel : class
    {
        TViewModel Create();
    }
}
