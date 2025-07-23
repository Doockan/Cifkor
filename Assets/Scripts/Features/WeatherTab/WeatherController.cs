using UniRx;
using System;
using Zenject;
using UnityEngine.UIElements;
using System.Threading;
using Assets.Scripts.Core.RequestQueue;
using Assets.Scripts.Core.Utils.WeatherHandle;
using Assets.Scripts.Features.MainTabs;
using UnityEngine;

namespace Assets.Scripts.Features.WeatherTab
{
    public class WeatherController : IInitializable, IDisposable
    {
        private readonly IWeatherApiService _weatherApiService;
        private readonly IRequestQueue _requestQueue;
        private readonly WeatherView _view;
        private readonly MainTabsController _mainTabsController;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private CancellationTokenSource _cts;
        private IDisposable _weatherSubscription;

        public WeatherController(IWeatherApiService weatherApiService, IRequestQueue requestQueue,
            WeatherView view, MainTabsController mainTabsController)
        {
            _weatherApiService = weatherApiService;
            _requestQueue = requestQueue;
            _view = view;
            _mainTabsController = mainTabsController;
        }

        public void Initialize()
        {
            _mainTabsController.OnTabChangeAction.Subscribe(OnTabActive).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
            DeactivateTab();
        }

        private void OnTabActive(ETabType type)
        {
            if (type == ETabType.Weather)
            {
                _view.Root.style.display = DisplayStyle.Flex;
                ActivateTab();
            }
            else
            {
                _view.Root.style.display = DisplayStyle.None;
                DeactivateTab();
            }
        }

        private void ActivateTab()
        {
            _cts = new CancellationTokenSource();

            _weatherSubscription = Observable
                .Interval(TimeSpan.FromSeconds(5))
                .StartWith(0)
                .TakeUntilDisable(_view)
                .Subscribe(_ => { EnqueueWeatherRequest(_cts.Token); });

            _weatherSubscription.AddTo(_disposables);
        }

        private void DeactivateTab()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _requestQueue.RemoveRequestsOfType<WeatherForecastModel>();
            _weatherSubscription?.Dispose();
        }

        private void EnqueueWeatherRequest(CancellationToken ct)
        {
            Debug.Log("UPDATE weather data");
            _requestQueue.Enqueue(async (token) =>
            {
                var forecast = await _weatherApiService.GetForecastAsync(token);
                _view.SetWeather(forecast);
            }, typeof(WeatherForecastModel), ct);
        }
    }
}