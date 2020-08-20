namespace SpaceLogistic.WpfView.ViewModel
{
    public interface IViewModelFactory<out TViewModel>
        where TViewModel : class
    {
        TViewModel Create();
    }
}
