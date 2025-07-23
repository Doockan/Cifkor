using Zenject;
using UnityEngine;
using Assets.Scripts.Models;
using Assets.Scripts.ScriptableObjects;

namespace Assets.Scripts.Features.ClickerTab
{
  public class ClickerController : IInitializable, ITickable
  {
    private readonly CurrencyModel _currency;
    private readonly EnergyModel _energy;
    private readonly ClickerView _view;
    private readonly CurrencyConfigSO _currencyConfig;
    private readonly EnergyConfigSO _energyConfig;
    private float _autoCollectTimer;
    private float _energyRestoreTimer;

    public ClickerController(ClickerView view, CurrencyConfigSO currencyConfig, EnergyConfigSO energyConfig)
    {
      _view = view;
      _currencyConfig = currencyConfig;
      _energyConfig = energyConfig;
      _currency = new CurrencyModel();
      _energy = new EnergyModel(_energyConfig.MaxEnergy);
    }

    public void Initialize()
    {
      _view.TapButton.onClick.AddListener(HandleClick);
      UpdateUI();
    }

    public void Tick()
    {
      _autoCollectTimer += Time.deltaTime;
      _energyRestoreTimer += Time.deltaTime;

      if (_autoCollectTimer >= _currencyConfig.AutoCollectInterval)
      {
        TryAutoCollect();
        _autoCollectTimer = 0;
      }

      if (_energyRestoreTimer >= _energyConfig.RestoreInterval)
      {
        _energy.Add(_energyConfig.RestoreAmount);
        _energyRestoreTimer = 0;
        UpdateUI();
      }
    }

    private void HandleClick()
    {
      if (_energy.TrySpend(1))
      {
        _currency.Add(_currencyConfig.ClickReward);
        // VFX, SFX, анимации
        UpdateUI();
      }
    }

    private void TryAutoCollect()
    {
      if (_energy.TrySpend(1))
      {
        _currency.Add(_currencyConfig.AutoCollectReward);
        // VFX, SFX, анимации
        UpdateUI();
      }
    }

    private void UpdateUI()
    {
      _view.SetCurrency(_currency.Amount);
      _view.SetEnergy(_energy.Energy);
    }
  }
}