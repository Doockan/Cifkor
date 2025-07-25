using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
  [CreateAssetMenu(menuName = "Config/EnergyConfigSO")]
  public class EnergyConfigSO : ScriptableObject
  {
    public int MaxEnergy = 1000;
    public float RestoreInterval = 10f;
    public int RestoreAmount = 10;
  }
}