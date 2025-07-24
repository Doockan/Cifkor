using UniRx;
using System;
using UnityEngine.UIElements;
using System.Threading;
using Assets.Scripts.API;
using Assets.Scripts.Core.RequestQueue;
using Assets.Scripts.Core.Utils.WeatherHandle;
using Assets.Scripts.Features.MainTabs;
using UnityEngine;

namespace Assets.Scripts.Features.WeatherTab
{
    public class WeatherController : ITabController, IDisposable
    {
        private readonly IWeatherApiService _weatherApiService;
        private readonly IRequestQueue _requestQueue;
        private readonly WeatherView _view;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private CancellationTokenSource _cts;
        private IDisposable _weatherSubscription;

        public ETabType TabType => ETabType.Weather;

        public WeatherController(IWeatherApiService weatherApiService, IRequestQueue requestQueue,
            WeatherView view)
        {
            _weatherApiService = weatherApiService;
            _requestQueue = requestQueue;
            _view = view;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public void ActivateTab()
        {
            _view.Root.style.display = DisplayStyle.Flex;
            _cts = new CancellationTokenSource();

            _weatherSubscription = Observable
                .Interval(TimeSpan.FromSeconds(5))
                .StartWith(0)
                .TakeUntilDisable(_view)
                .Subscribe(_ => { EnqueueWeatherRequest(_cts.Token); });

            _weatherSubscription.AddTo(_disposables);
        }

        public void DeactivateTab()
        {
            _view.Root.style.display = DisplayStyle.None;
            _cts?.Cancel();
            _cts?.Dispose();
            _requestQueue.RemoveRequestsOfType<WeatherForecastDataModel>();
            _weatherSubscription?.Dispose();
        }

        private void EnqueueWeatherRequest(CancellationToken ct)
        {
            Debug.Log("UPDATE weather data");
            _requestQueue.Enqueue(async (token) =>
            {
                var model = await _weatherApiService.GetForecastAsync(token);
                var iconTexture = await _weatherApiService.GetWeatherIconAsync(model.IconUrl, ct);

                _view.SetWeather(model, iconTexture);
            }, typeof(WeatherForecastDataModel), ct);
        }
    }
}