using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Assets.Scripts.Core.Utils.WeatherHandle
{
    public class WeatherApiService : IWeatherApiService
    {
        private const string ApiUrl = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";

        public async Task<WeatherForecastModel> GetForecastAsync(CancellationToken cancellationToken)
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

#if UNITY_2022_1_OR_NEWER
                if (request.result != UnityWebRequest.Result.Success)
#else
            if (request.isNetworkError || request.isHttpError)
#endif
                {
                    throw new System.Exception($"Weather request error: {request.error}");
                }

                // Преобразование JSON в модель
                var json = request.downloadHandler.text;
                return WeatherForecastModel.FromJson(json);
            }
        }
    }
}