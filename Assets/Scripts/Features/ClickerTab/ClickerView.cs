using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Features.ClickerTab
{
    public class ClickerView : MonoBehaviour
    {
        [SerializeField] private UIDocument _uIDocument;

        private VisualElement Root => _uIDocument.rootVisualElement;
        public Button ClickerButton => Root.Q<Button>("ClickerButton");
        public Label CurrencyLabel => Root.Q<Label>("CurrencyLabel");
        public Label EnergyLabel => Root.Q<Label>("EnergyLabel");
    }
}