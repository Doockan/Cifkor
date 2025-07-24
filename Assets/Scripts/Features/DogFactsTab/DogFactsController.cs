using System;
using System.Threading;
using UniRx;
using Assets.Scripts.API;
using Assets.Scripts.Core.Utils;
using Assets.Scripts.Core.RequestQueue;
using Assets.Scripts.Features.MainTabs;

namespace Assets.Scripts.Features.DogFactsTab
{
    public class DogFactsController : ITabController, IDisposable
    {
        private readonly IDogApiService _dogApiService;
        private readonly IRequestQueue _requestQueue;
        private readonly DogFactsView _view;
        private CancellationTokenSource _cts;
        private IDisposable _breedClickSubscription;

        public ETabType TabType => ETabType.Dog;

        public DogFactsController(IDogApiService dogApiService, IRequestQueue requestQueue,
            DogFactsView dogFactsView)
        {
            _dogApiService = dogApiService;
            _requestQueue = requestQueue;
            _view = dogFactsView;
        }

        public void Dispose()
        {
            _breedClickSubscription?.Dispose();
        }

        public void ActivateTab()
        {
            _view.Root.style.display = UnityEngine.UIElements.DisplayStyle.Flex;
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

        public void DeactivateTab()
        {
            _view.Root.style.display = UnityEngine.UIElements.DisplayStyle.None;
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