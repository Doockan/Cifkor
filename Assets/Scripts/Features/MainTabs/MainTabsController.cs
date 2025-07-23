using Zenject;
using Assets.Scripts.Features.ClickerTab;
using Assets.Scripts.Features.DogFactsTab;
using Assets.Scripts.Features.WeatherTab;

namespace Assets.Scripts.Features.MainTabs
{
    public class MainTabsController : IInitializable
    {
        private readonly MainTabsView _mainTabsView;
        private readonly ClickerController _clickerController;
        private readonly WeatherController _weatherController;
        private readonly DogFactsController _dogFactsController;

        public MainTabsController(
            MainTabsView mainTabsView,
            ClickerController clickerController,
            WeatherController weatherController,
            DogFactsController dogFactsController)
        {
            _mainTabsView = mainTabsView;
            _clickerController = clickerController;
            _weatherController = weatherController;
            _dogFactsController = dogFactsController;
        }

        public void Initialize()
        {
            _mainTabsView.TabClicker.clicked += () => SwitchTab(ETabType.Clicker);
            _mainTabsView.TabWeather.clicked += () => SwitchTab(ETabType.Weather);
            _mainTabsView.TabDog.clicked += () => SwitchTab(ETabType.Dog);

            SwitchTab(ETabType.Clicker);
        }

        private void SwitchTab(ETabType tab)
        {
            _clickerController.OnTabActivated(tab == ETabType.Clicker);
            _weatherController.OnTabActivated(tab == ETabType.Weather);
            _dogFactsController.OnTabActivated(tab == ETabType.Dog);

            SelectButton(tab);
        }

        private void SelectButton(ETabType tab)
        {
            _mainTabsView.TabClicker.RemoveFromClassList("active");
            _mainTabsView.TabWeather.RemoveFromClassList("active");
            _mainTabsView.TabDog.RemoveFromClassList("active");
            if (tab == ETabType.Clicker) _mainTabsView.TabClicker.AddToClassList("active");
            if (tab == ETabType.Weather) _mainTabsView.TabWeather.AddToClassList("active");
            if (tab == ETabType.Dog) _mainTabsView.TabDog.AddToClassList("active");
        }
    }
}