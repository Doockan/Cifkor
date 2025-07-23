using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Features.DogFactsTab
{
    public class DogFactsView : MonoBehaviour
    {
        [SerializeField] private UIDocument _uIDocument;

        public VisualElement Root => _uIDocument.rootVisualElement.Q<VisualElement>("DogTab");
    }
}