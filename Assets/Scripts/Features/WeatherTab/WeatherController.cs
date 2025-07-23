using UnityEngine.UIElements;

namespace Assets.Scripts.Features.WeatherTab
{
    public class WeatherController
    {
        private readonly WeatherView _view;

        public WeatherController(WeatherView weatherView)
        {
            _view = weatherView;
        }

        public void OnTabActivated(bool active)
        {
            if (active)
            {
                _view.Root.style.display = DisplayStyle.Flex;
            }
            else
            {
                _view.Root.style.display = DisplayStyle.None;
            }
        }
    }
}