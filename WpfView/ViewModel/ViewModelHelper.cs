namespace SpaceLogistic.WpfView.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using SpaceLogistic.Core.Model;

    public static class ViewModelHelper
    {
        public static ObservableCollection<TViewModel> Update<TViewModel, TModel>(
            ObservableCollection<TViewModel> viewModels, 
            IEnumerable<TModel> models,
            Func<TModel, TViewModel> create,
            Action<TModel, TViewModel> update)
            where TViewModel : IIdentity
            where TModel : IIdentity
        {
            var viewModelsByIds = viewModels.ToDictionary(vm => vm.Id);
            var modelsByIds = models.ToDictionary(m => m.Id);

            var updatedViewModels = new List<TViewModel>();
            var collectionChanged = false;

            foreach (var viewModel in viewModelsByIds.Values)
            {
                if (modelsByIds.TryGetValue(viewModel.Id, out var model))
                {
                    update(model, viewModel);
                    updatedViewModels.Add(viewModel);
                }
                else
                {
                    collectionChanged = true;
                }
            }

            foreach (var model in modelsByIds.Values)
            {
                if (!viewModelsByIds.ContainsKey(model.Id))
                {
                    var viewModel = create(model);
                    updatedViewModels.Add(viewModel);
                    collectionChanged = true;
                }
            }

            if (!collectionChanged)
            {
                return viewModels;
            }

            return new ObservableCollection<TViewModel>(updatedViewModels);
        }
    }
}