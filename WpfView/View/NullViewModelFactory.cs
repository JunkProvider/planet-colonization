namespace SpaceLogistic.WpfView.View
{
    using System;

    public sealed class NullViewModelFactory<TViewModel> : IViewModelFactory<TViewModel>
        where TViewModel : class
    {
        public TViewModel Create()
        {
            throw new NotSupportedException();
        }

        public static IViewModelFactory<TViewModel> Instance { get; } = new NullViewModelFactory<TViewModel>();
    }
}
