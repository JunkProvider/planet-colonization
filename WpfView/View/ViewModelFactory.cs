namespace SpaceLogistic.WpfView.View
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    public sealed class ViewModelFactory<TViewModel> : IViewModelFactory<TViewModel>
        where TViewModel : class
    {
        private readonly IServiceProvider serviceProvider;

        public ViewModelFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public TViewModel Create()
        {
            return this.serviceProvider.GetRequiredService<TViewModel>();
        }
    }
}