using System;
using Zenject;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
using Assets.Scripts.Features.MainTabs;
using Assets.Scripts.ScriptableObjects;

namespace Assets.Scripts.Features.ClickerTab
{
  public class ClickerController : ITabController, IInitializable, IDisposable, ITickable
  {
    private readonly ClickerView _view;
    private readonly CurrencyConfigSO _currencyConfig;
    private readonly EnergyConfigSO _energyConfig;

    private int _currency;
    private int _energy;
    private float _autoCollectTimer;
    private float _energyRestoreTimer;
    private Button _clickerButton;

    public ETabType TabType => ETabType.Clicker;

    public ClickerController(ClickerView view, CurrencyConfigSO currencyConfig, EnergyConfigSO energyConfig)
    {
      _view = view;
      _currencyConfig = currencyConfig;
      _energyConfig = energyConfig;
    }

    public void Initialize()
    {
      _energy = _energyConfig.MaxEnergy;
      _view.CurrencyLabel.text = $"Валюта: {_currency}";
      _view.EnergyLabel.text = $"Энергия: {_energy}";
      _clickerButton = _view.ClickerButton;
      _clickerButton.clicked += OnClick;
    }

    public void Dispose()
    {
      if (_clickerButton != null)
        _clickerButton.clicked -= OnClick;
    }

    public void Tick()
    {
      _autoCollectTimer += Time.unscaledDeltaTime;
      if (_autoCollectTimer >= _currencyConfig.AutoCollectInterval)
      {
        TryAutoCollect();
        _autoCollectTimer = 0f;
      }

      _energyRestoreTimer += Time.unscaledDeltaTime;
      if (_energyRestoreTimer >= _energyConfig.RestoreInterval)
      {
        RestoreEnergy();
        _energyRestoreTimer = 0f;
      }
    }


    public void ActivateTab()
    {
      _view.Root.style.display = DisplayStyle.Flex;
    }

    public void DeactivateTab()
    {
      _view.Root.style.display = DisplayStyle.None;
    }


    private void OnClick()
    {
      if (_energy > 0)
      {
        _currency += _currencyConfig.ClickReward;
        _energy -= 1;
        UpdateUI();
        PlayVfx();
      }
      else
      {
        // Можно показать сообщение "Нет энергии"
      }
    }

    private void TryAutoCollect()
    {
      if (_energy > 0)
      {
        _currency += _currencyConfig.AutoCollectReward;
        _energy -= 1;
        UpdateUI();
        PlayVfx();
      }
    }

    private void PlayVfx()
    {
      _view.PlayCurrencyFlyVfx();
      _view.PlayParticleVfx(Random.Range(50, 100));
    }

    private void RestoreEnergy()
    {
      int newEnergy = Mathf.Min(_energy + _energyConfig.RestoreAmount, _energyConfig.MaxEnergy);
      if (newEnergy != _energy)
      {
        _energy = newEnergy;
        UpdateUI();
      }
    }

    private void UpdateUI()
    {
      _view.CurrencyLabel.text = $"Валюта: {_currency}";
      _view.EnergyLabel.text = $"Энергия: {_energy}";
    }
  }
}