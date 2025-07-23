using UnityEngine;

namespace Assets.Scripts.Models
{
  public class EnergyModel
  {
    public int Energy { get; private set; }

    public int MaxEnergy { get; }

    public EnergyModel(int maxEnergy) { MaxEnergy = maxEnergy; Energy = maxEnergy; }

    public bool TrySpend(int value)
    {
      if (Energy >= value)
      {
        Energy -= value;
        return true;
      }
      return false;
    }

    public void Add(int value)
    {
      Energy = Mathf.Min(Energy + value, MaxEnergy);
    }
  }
}