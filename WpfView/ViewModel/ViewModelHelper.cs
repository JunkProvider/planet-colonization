namespace SpaceLogistic.WpfView.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using SpaceLogistic.Core.Model;

    public static class ViewModelHelper
    {
        public static ObservableCollection<TViewModel> UpdateCollection<TViewModel, TModel>(
            ObservableCollection<TViewModel> viewModels,
            IEnumerable<TModel> models,
            Func<TModel, TViewModel> create,
            Action<TModel, TViewModel> update)
            where TViewModel : IIdentity where TModel : IIdentity
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
                    update(model, viewModel);
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

        public static TViewModel Update<TViewModel, TModel>(
            TViewModel viewModel,
            TModel model,
            Func<TModel, TViewModel> create,
            Action<TModel, TViewModel> update)
            where TViewModel : class, IIdentity
            where TModel : class, IIdentity
        {
            if (model == null && viewModel == null)
            {
                return null;
            }

            if (model == null)
            {
                return null;
            }

            if (viewModel == null)
            {
                viewModel = create(model);
                update(model, viewModel);
                return viewModel;
            }

            if (viewModel.Id == model.Id)
            {
                update(model, viewModel);
                return viewModel;
            }

            viewModel = create(model);
            update(model, viewModel);
            return viewModel;
        }
    }
}