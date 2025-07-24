using UnityEngine;
using UnityEngine.UIElements;
using Assets.Scripts.Core.Utils.WeatherHandle;

namespace Assets.Scripts.Features.WeatherTab
{
    public class WeatherView : MonoBehaviour
    {
        [SerializeField] private UIDocument _uIDocument;

        public VisualElement Root => _uIDocument.rootVisualElement.Q<VisualElement>("WeatherTab");

        public void SetWeather(WeatherForecastDataModel model)
        {
            var label = _uIDocument.rootVisualElement.Q<Label>("WeatherLabel");
            var icon = _uIDocument.rootVisualElement.Q<Image>("WeatherIcon");

            label.text = $"Сегодня - {model.Temperature}";
            // Загрузить иконку по URL (можно через UnityWebRequestTexture)
            // icon.sprite = ... ;
        }
    }
}