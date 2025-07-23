using Zenject;
using Assets.Scripts.Core.RequestQueue;
using Assets.Scripts.Features.ClickerTab;
using Assets.Scripts.Features.DogFactsTab;
using Assets.Scripts.Features.WeatherTab;

namespace Assets.Scripts.Installers
{
  public class GameInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      // Очередь запросов
      Container.Bind<IRequestQueue>().To<RequestQueue>().AsSingle();

      // Контроллеры вкладок
      Container.BindInterfacesAndSelfTo<ClickerController>().AsSingle();
      Container.BindInterfacesAndSelfTo<WeatherController>().AsSingle();
      Container.BindInterfacesAndSelfTo<DogFactsController>().AsSingle();

      // Сервисы API
      // Container.Bind<IWeatherApiService>().To<WeatherApiService>().AsSingle();
      // Container.Bind<IDogApiService>().To<DogApiService>().AsSingle();

      // ObjectPool (например, для попапов, VFX)
      // Container.BindFactory<PopupView, PopupView.Factory>().FromComponentInNewPrefabResource("PopupView");
    }
  }
}