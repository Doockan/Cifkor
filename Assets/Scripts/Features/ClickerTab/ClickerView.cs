using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Features.ClickerTab
{
    public class ClickerView : MonoBehaviour
    {
      [SerializeField] private Button _tapButton;

      public Button TapButton => _tapButton;

      public void SetCurrency(int amount)
      {
      }

      public void SetEnergy(int energy)
      {
      }
    }
}