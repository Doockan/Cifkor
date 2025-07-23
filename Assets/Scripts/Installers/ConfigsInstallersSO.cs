using Zenject;
using UnityEngine;
using Assets.Scripts.ScriptableObjects;

namespace Assets.Scripts.Installers
{
  [CreateAssetMenu(menuName = "Config/Installers/ConfigsInstallerSO")]
  public class ConfigsInstallerSO : ScriptableObjectInstaller
  {
    public CurrencyConfigSO CurrencyConfig;
    public EnergyConfigSO EnergyConfig;

    public override void InstallBindings()
    {
      Container.BindInstance(CurrencyConfig).AsSingle();
      Container.BindInstance(EnergyConfig).AsSingle();
    }
  }
}