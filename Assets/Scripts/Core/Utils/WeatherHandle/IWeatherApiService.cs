using System.Threading;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Utils.WeatherHandle
{
    public interface IWeatherApiService
    {
        Task<WeatherForecastModel> GetForecastAsync(CancellationToken cancellationToken);
    }
}