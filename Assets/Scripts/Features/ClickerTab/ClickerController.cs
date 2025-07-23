using Zenject;
using UnityEngine;
using Assets.Scripts.ScriptableObjects;

namespace Assets.Scripts.Features.ClickerTab
{
  public class ClickerController : IInitializable, ITickable
  {
    private readonly ClickerView _view;
    private readonly CurrencyConfigSO _currencyConfig;
    private readonly EnergyConfigSO _energyConfig;

    private int _currency = 0;
    private int _energy;
    private float _autoCollectTimer = 0f;
    private float _energyRestoreTimer = 0f;

    public ClickerController(
      ClickerView view,
      CurrencyConfigSO currencyConfig,
      EnergyConfigSO energyConfig)
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
      _view.ClickerButton.clicked += OnClick;

      // Доп. VFX/Audio (заглушки) можно добавить здесь
    }

    public void Tick()
    {
      // Автосбор валюты
      _autoCollectTimer += Time.unscaledDeltaTime;
      if (_autoCollectTimer >= _currencyConfig.AutoCollectInterval)
      {
        TryAutoCollect();
        _autoCollectTimer = 0f;
      }

      // Восстановление энергии
      _energyRestoreTimer += Time.unscaledDeltaTime;
      if (_energyRestoreTimer >= _energyConfig.RestoreInterval)
      {
        RestoreEnergy();
        _energyRestoreTimer = 0f;
      }
    }

    void OnClick()
    {
      if (_energy > 0)
      {
        _currency += _currencyConfig.ClickReward;
        _energy -= 1;
        UpdateUI();
        // VFX/Audio (например, PlayClickVFX())
      }
      else
      {
        // Можно показать сообщение "Нет энергии"
      }
    }

    void TryAutoCollect()
    {
      if (_energy > 0)
      {
        _currency += _currencyConfig.AutoCollectReward;
        _energy -= 1;
        UpdateUI();
        // VFX/Audio (например, PlayAutoCollectVFX())
      }
    }

    void RestoreEnergy()
    {
      int newEnergy = Mathf.Min(_energy + _energyConfig.RestoreAmount, _energyConfig.MaxEnergy);
      if (newEnergy != _energy)
      {
        _energy = newEnergy;
        UpdateUI();
      }
    }

    void UpdateUI()
    {
      _view.CurrencyLabel.text = $"Валюта: {_currency}";
      _view.EnergyLabel.text = $"Энергия: {_energy}";
    }
  }
}