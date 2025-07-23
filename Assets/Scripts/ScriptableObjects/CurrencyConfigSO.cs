using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
  [CreateAssetMenu(menuName = "Config/CurrencyConfigSO")]

  public class CurrencyConfigSO : ScriptableObject
  {
    public int ClickReward = 1;
    public int AutoCollectReward = 1;
    public float AutoCollectInterval = 3f;
  }
}