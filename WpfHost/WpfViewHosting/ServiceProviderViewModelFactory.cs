namespace SpaceLogistic.WpfHost.WpfViewHosting
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    using SpaceLogistic.WpfView.ViewModel;

    public sealed class ServiceProviderViewModelFactory<TViewModel> : IViewModelFactory<TViewModel>
        where TViewModel : class
    {
        private readonly IServiceProvider serviceProvider;

        public ServiceProviderViewModelFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public TViewModel Create()
        {
            return this.serviceProvider.GetRequiredService<TViewModel>();
        }
    }
}