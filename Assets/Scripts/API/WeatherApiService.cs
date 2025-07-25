using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using Assets.Scripts.Core.Utils;

namespace Assets.Scripts.API
{
    public class WeatherApiService : IWeatherApiService
    {
        private const string ApiUrl = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";

        public async Task<WeatherForecastDataModel> GetForecastAsync(CancellationToken cancellationToken)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(ApiUrl))
            {
                var operation = request.SendWebRequest();

                while (!operation.isDone)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        request.Abort();
                        cancellationToken.ThrowIfCancellationRequested();
                    }

                    await Task.Yield();
                }

                if (request.isNetworkError || request.isHttpError)
                {
                    throw new System.Exception($"Weather request error: {request.error}");
                }

                // Преобразование JSON в модель
                var json = request.downloadHandler.text;
                return WeatherForecastDataModel.FromJson(json);
            }
        }

        public async Task<Texture2D> GetWeatherIconAsync(string url, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(url)) return null;

            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
            {
                var operation = request.SendWebRequest();

                while (!operation.isDone)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        request.Abort();
                        cancellationToken.ThrowIfCancellationRequested();
                    }

                    await Task.Yield();
                }

                if (request.isNetworkError || request.isHttpError)
                {
                    Debug.LogWarning($"Weather icon request error: {request.error}");
                    return null;
                }

                return DownloadHandlerTexture.GetContent(request);
            }
        }
    }
}