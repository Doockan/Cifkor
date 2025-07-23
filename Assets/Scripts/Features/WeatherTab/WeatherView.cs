using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Features.WeatherTab
{
    public class WeatherView : MonoBehaviour
    {    
        [SerializeField] private UIDocument _uIDocument;

        public VisualElement Root => _uIDocument.rootVisualElement.Q<VisualElement>("WeatherTab");
    }
}