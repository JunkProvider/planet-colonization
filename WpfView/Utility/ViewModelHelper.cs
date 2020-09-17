namespace SpaceLogistic.WpfView.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using SpaceLogistic.Core.Model;

    public static class ViewModelHelper
    {
        public static ObservableCollection<TViewModel> UpdateCollectionByReference<TViewModel, TModel>(
            ObservableCollection<TViewModel> viewModels,
            IEnumerable<TModel> models,
            Func<TModel, TViewModel> create,
            Action<TModel, TViewModel> update)
        {
            return InternalUpdateCollection<TViewModel, TModel, object>(viewModels, models, m => m, vm => vm, create, update);
        }

        public static ObservableCollection<TViewModel> UpdateCollectionByIdentity<TViewModel, TModel>(
            ObservableCollection<TViewModel> viewModels,
            IEnumerable<TModel> models,
            Func<TModel, TViewModel> create,
            Action<TModel, TViewModel> update)
            where TViewModel : IIdentity where TModel : IIdentity
        {
            return InternalUpdateCollection(viewModels, models, m => m.Id, vm => vm.Id, create, update);

            /*var viewModelsByIds = viewModels.ToDictionary(vm => vm.Id);
            var modelList = models.ToList();
            var modelsByIds = modelList.ToDictionary(m => m.Id);
            
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
            
            var updatedViewModelsByIds = updatedViewModels.ToDictionary(viewModel => viewModel.Id);
            var sortedViewModels = new List<TViewModel>();
            var collectionOrderChanged = false;

            for (var i = 0; i < modelList.Count; i++)
            {
                var model = modelList[i];
                var viewModel = updatedViewModels[i];

                if (viewModel.Id == model.Id)
                {
                    sortedViewModels.Add(viewModel);
                    continue;
                }

                sortedViewModels.Add(updatedViewModelsByIds[model.Id]);
                collectionOrderChanged = true;
            }

            if (!collectionChanged && !collectionOrderChanged)
            {
                return viewModels;
            }

            return new ObservableCollection<TViewModel>(sortedViewModels);*/
        }

        private static ObservableCollection<TViewModel> InternalUpdateCollection<TViewModel, TModel, TKey>(
            ObservableCollection<TViewModel> viewModels,
            IEnumerable<TModel> models,
            Func<TModel, TKey> modelKeyFunc,
            Func<TViewModel, TKey> viewModelKeyFunc,
            Func<TModel, TViewModel> create,
            Action<TModel, TViewModel> update)
        {
            var viewModelsByIds = viewModels.ToDictionary(viewModelKeyFunc);
            var modelList = models.ToList();
            var modelsByIds = modelList.ToDictionary(modelKeyFunc);

            var updatedViewModels = new List<TViewModel>();
            var collectionChanged = false;

            foreach (var viewModel in viewModelsByIds.Values)
            {
                var id = viewModelKeyFunc(viewModel);

                if (modelsByIds.TryGetValue(id, out var model))
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
                var id = modelKeyFunc(model);

                if (!viewModelsByIds.ContainsKey(id))
                {
                    var viewModel = create(model);
                    update(model, viewModel);
                    updatedViewModels.Add(viewModel);
                    collectionChanged = true;
                }
            }

            var updatedViewModelsByIds = updatedViewModels.ToDictionary(viewModelKeyFunc);
            var sortedViewModels = new List<TViewModel>();
            var collectionOrderChanged = false;

            for (var i = 0; i < modelList.Count; i++)
            {
                var model = modelList[i];
                var viewModel = updatedViewModels[i];

                var modelId = modelKeyFunc(model);
                var viewModelId = viewModelKeyFunc(viewModel);

                if (viewModelId.Equals(modelId))
                {
                    sortedViewModels.Add(viewModel);
                    continue;
                }

                sortedViewModels.Add(updatedViewModelsByIds[modelId]);
                collectionOrderChanged = true;
            }

            if (!collectionChanged && !collectionOrderChanged)
            {
                return viewModels;
            }

            return new ObservableCollection<TViewModel>(sortedViewModels);
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