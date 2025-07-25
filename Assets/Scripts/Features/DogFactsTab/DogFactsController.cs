using UniRx;
using System;
using System.Threading;
using Assets.Scripts.API;
using Assets.Scripts.Core.Utils;
using Assets.Scripts.Core.RequestQueue;
using Assets.Scripts.Features.MainTabs;
using Zenject;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Features.DogFactsTab
{
    public class DogFactsController : ITickable, ITabController, IDisposable
    {
        private readonly IDogApiService _dogApiService;
        private readonly IRequestQueue _requestQueue;
        private readonly DogFactsView _view;
        private CancellationTokenSource _cts;
        private IDisposable _breedClickSubscription;
        private float _angle;
        private bool _loaderOnShow;
        public ETabType TabType => ETabType.Dog;

        public DogFactsController(IDogApiService dogApiService, IRequestQueue requestQueue,
            DogFactsView dogFactsView)
        {
            _dogApiService = dogApiService;
            _requestQueue = requestQueue;
            _view = dogFactsView;
        }

        public void Tick()
        {
            if (_loaderOnShow)
            {
                _angle += 360 * Time.deltaTime / 0.8f;
                _view.Loader.style.rotate = new StyleRotate(new Rotate(_angle));
            }
        }

        public void Dispose()
        {
            _breedClickSubscription?.Dispose();
        }

        public void ActivateTab()
        {
            _view.Root.style.display = DisplayStyle.Flex;
            _cts = new CancellationTokenSource();
            Loader(true);
            _view.HideBreedFactPopup();

            _requestQueue.Enqueue(async (ct) =>
            {
                var breedsModel = await _dogApiService.GetBreedsAsync(ct);
                Loader(false);
                _view.SetBreedsList(breedsModel);

                _breedClickSubscription?.Dispose();
                _breedClickSubscription = _view.OnBreedClicked
                    .Subscribe(breedId => RequestBreedFact(breedId));
            }, typeof(DogBreedDataModel), _cts.Token);
        }

        public void DeactivateTab()
        {
            _view.ClearBreedsList();
            _view.Root.style.display = DisplayStyle.None;
            _cts?.Cancel();
            _cts?.Dispose();
            _requestQueue.RemoveRequestsOfType<DogBreedDataModel>();

            Loader(false);
            _view.HideBreedFactPopup();

            _breedClickSubscription?.Dispose();
        }

        private void RequestBreedFact(string breedId)
        {
            _requestQueue.RemoveRequestsOfType<DogBreedDataModel>();
            _view.HideBreedFactPopup();

            _requestQueue.Enqueue(async (ct) =>
            {
                var breedModel = await _dogApiService.GetBreedAsync(breedId, ct);

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

        private void Loader(bool show)
        {
            _view.ShowLoader(show);
            _loaderOnShow = show;
        }
    }
}