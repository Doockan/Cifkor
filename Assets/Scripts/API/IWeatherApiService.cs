using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.Core.Utils;

namespace Assets.Scripts.API
{
    public interface IWeatherApiService
    {
        Task<WeatherForecastDataModel> GetForecastAsync(CancellationToken cancellationToken);
        Task<Texture2D> GetWeatherIconAsync(string url, CancellationToken cancellationToken);
    }
}