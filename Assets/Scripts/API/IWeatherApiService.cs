using System.Threading;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Utils.WeatherHandle
{
    public interface IWeatherApiService
    {
        Task<WeatherForecastDataModel> GetForecastAsync(CancellationToken cancellationToken);
    }
}