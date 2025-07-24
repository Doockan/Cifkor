using Zenject;
using System.Linq;
using System.Collections.Generic;

namespace Assets.Scripts.Features.MainTabs
{
    public class MainTabsController : IInitializable
    {
        private readonly MainTabsView _mainTabsView;
        private readonly Dictionary<ETabType, ITabController> _tabControllers;

        private ETabType _currentActiveTab;

        public MainTabsController(MainTabsView mainTabsView, List<ITabController> tabControllers)
        {
            _mainTabsView = mainTabsView;
            _tabControllers = tabControllers.ToDictionary(c => c.TabType);
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
            if (_currentActiveTab == tab)
                return;

            if (_tabControllers.TryGetValue(_currentActiveTab, out var prevController))
            {
                prevController.DeactivateTab();
            }

            if (_tabControllers.TryGetValue(tab, out var newController))
            {
                newController.ActivateTab();
            }

            _currentActiveTab = tab;
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