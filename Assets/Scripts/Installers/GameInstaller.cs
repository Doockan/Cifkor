using Zenject;
using UnityEngine;
using Assets.Scripts.Core.RequestQueue;
using Assets.Scripts.Core.Utils.WeatherHandle;
using Assets.Scripts.Features.ClickerTab;
using Assets.Scripts.Features.DogFactsTab;
using Assets.Scripts.Features.MainTabs;
using Assets.Scripts.Features.WeatherTab;
using Assets.Scripts.API;

namespace Assets.Scripts.Installers
{
  public class GameInstaller : MonoInstaller
  {
    [Header("Views on Scene")] [SerializeField]
    private ClickerView _clickerView;

    [SerializeField] private WeatherView _weatherView;
    [SerializeField] private DogFactsView _dogFactsView;
    [SerializeField] private MainTabsView _mainTabsView;

    public override void InstallBindings()
    {
      Container.Bind<IRequestQueue>().To<RequestQueue>().AsSingle();

      ControllersBind();
      ViewsBind();

      Container.Bind<IWeatherApiService>().To<WeatherApiService>().AsSingle();
      Container.Bind<IDogApiService>().To<DogApiService>().AsSingle();

      // ObjectPool (например, для попапов, VFX)
      // Container.BindFactory<PopupView, PopupView.Factory>().FromComponentInNewPrefabResource("PopupView");
    }

    private void ViewsBind()
    {
      Container.BindInstance(_clickerView).AsSingle();
      Container.BindInstance(_weatherView).AsSingle();
      Container.BindInstance(_dogFactsView).AsSingle();
      Container.BindInstance(_mainTabsView).AsSingle();
    }


    private void ControllersBind()
    {
      Container.BindInterfacesAndSelfTo<ClickerController>().AsSingle();
      Container.BindInterfacesAndSelfTo<DogFactsController>().AsSingle();
      Container.BindInterfacesAndSelfTo<WeatherController>().AsSingle();
      Container.BindInterfacesAndSelfTo<MainTabsController>().AsSingle();
    }
  }
}