using UniRx;
using System;
using Zenject;
using UnityEngine;
using UnityEngine.UIElements;
using Assets.Scripts.Features.MainTabs;
using Assets.Scripts.ScriptableObjects;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Features.ClickerTab
{
  public class ClickerController : IInitializable, IDisposable, ITickable
  {
    private readonly ClickerView _view;
    private readonly CurrencyConfigSO _currencyConfig;
    private readonly EnergyConfigSO _energyConfig;
    private readonly MainTabsController _mainTabsController;
    private readonly CompositeDisposable _disposables = new CompositeDisposable();

    private int _currency = 0;
    private int _energy;
    private float _autoCollectTimer = 0f;
    private float _energyRestoreTimer = 0f;
    private Button _clickerButton;

    public ClickerController(ClickerView view, CurrencyConfigSO currencyConfig, EnergyConfigSO energyConfig,
      MainTabsController mainTabsController)
    {
      _view = view;
      _currencyConfig = currencyConfig;
      _energyConfig = energyConfig;
      _mainTabsController = mainTabsController;
    }

    public void Initialize()
    {
      _mainTabsController.OnTabChangeAction.Subscribe(OnTabActivated).AddTo(_disposables);
      _energy = _energyConfig.MaxEnergy;
      _view.CurrencyLabel.text = $"Валюта: {_currency}";
      _view.EnergyLabel.text = $"Энергия: {_energy}";
      _clickerButton = _view.ClickerButton;
      _clickerButton.clicked += OnClick;
    }

    public void Dispose()
    {
      _disposables.Dispose();
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

    private void OnTabActivated(ETabType type)
    {
      if (type == ETabType.Clicker)
      {
        _view.Root.style.display = DisplayStyle.Flex;
      }
      else
      {
        _view.Root.style.display = DisplayStyle.None;
      }
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