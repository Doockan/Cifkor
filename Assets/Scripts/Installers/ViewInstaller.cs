using Zenject;
using UnityEngine;
using Assets.Scripts.Features.ClickerTab;

namespace Assets.Scripts.Installers
{
  public class ViewInstaller : MonoInstaller
  {
    [Header("Views on Scene")]
    public ClickerView ClickerView;
    // public WeatherView WeatherView;
    // public DogFactsView DogFactsView;

    public override void InstallBindings()
    {
      Container.BindInstance(ClickerView).AsSingle();
      // Container.BindInstance(WeatherView).AsSingle();
      // Container.BindInstance(DogFactsView).AsSingle();
    }
  }
}