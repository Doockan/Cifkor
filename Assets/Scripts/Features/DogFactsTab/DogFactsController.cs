using System;
using System.Diagnostics;
using System.Threading;
using UniRx;
using Zenject;
using Assets.Scripts.API;
using Assets.Scripts.Core.Utils;
using Assets.Scripts.Core.RequestQueue;
using Assets.Scripts.Features.MainTabs;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Features.DogFactsTab
{
    public class DogFactsController : IInitializable, IDisposable
    {
        private readonly IDogApiService _dogApiService;
        private readonly IRequestQueue _requestQueue;
        private readonly DogFactsView _view;
        private readonly MainTabsController _mainTabsController;
        private CancellationTokenSource _cts;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private IDisposable _breedClickSubscription;

        public DogFactsController(IDogApiService dogApiService, IRequestQueue requestQueue,
            DogFactsView dogFactsView, MainTabsController mainTabsController)
        {
            _dogApiService = dogApiService;
            _requestQueue = requestQueue;
            _view = dogFactsView;
            _mainTabsController = mainTabsController;
        }

        public void Initialize()
        {
            _mainTabsController.OnTabChangeAction
                .Subscribe(OnTabActive)
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _breedClickSubscription?.Dispose();
            _disposables.Dispose();
        }

        private void OnTabActive(ETabType type)
        {
            if (type == ETabType.Dog)
            {
                _view.Root.style.display = UnityEngine.UIElements.DisplayStyle.Flex;
                ActivateTab();
            }
            else
            {
                _view.Root.style.display = UnityEngine.UIElements.DisplayStyle.None;
                DeactivateTab();
            }
        }

        private void ActivateTab()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();

            _view.ShowLoader(true);
            _view.ShowFactLoader(false);
            _view.HideBreedFactPopup();

            _requestQueue.Enqueue(async (ct) =>
            {
                var breedsModel = await _dogApiService.GetBreedsAsync(ct);
                _view.ShowLoader(false);
                _view.SetBreedsList(breedsModel);

                _breedClickSubscription?.Dispose();
                _breedClickSubscription = _view.OnBreedClicked
                    .Subscribe(breedId => RequestBreedFact(breedId));
            }, typeof(DogBreedDataModel), _cts.Token);
        }

        private void DeactivateTab()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _requestQueue.RemoveRequestsOfType<DogBreedDataModel>();

            _view.ShowLoader(false);
            _view.ShowFactLoader(false);
            _view.HideBreedFactPopup();

            _breedClickSubscription?.Dispose();
        }

        private void RequestBreedFact(string breedId)
        {
            _requestQueue.RemoveRequestsOfType<DogBreedDataModel>();
            _view.ShowFactLoader(true);
            _view.HideBreedFactPopup();

            _requestQueue.Enqueue(async (ct) =>
            {
                var breedModel = await _dogApiService.GetBreedAsync(breedId, ct);
                _view.ShowFactLoader(false);

                Debug.Log($"{breedModel.Data.Attributes.Name}");

                if (breedModel.Data != null)
                {
                    var attributes = breedModel.Data.Attributes;
                    string description = attributes.Description;
                    description += $"\nЖизнь: {attributes.Life.Min}-{attributes.Life.Max} лет";
                    description += $"\nВес самца: {attributes.MaleWeight.Min}-{attributes.MaleWeight.Max} кг";
                    description += $"\nВес самки: {attributes.FemaleWeight.Min}-{attributes.FemaleWeight.Max} кг";
                    description += $"\nГипоаллергенная: {(attributes.Hypoallergenic ? "Да" : "Нет")}";

                    _view.ShowBreedFactPopup(attributes.Name, description);
                }
                else
                {
                    _view.ShowBreedFactPopup("Ошибка", "Факт не найден.");
                }
            }, typeof(DogBreedDataModel), _cts.Token);
        }
    }
}