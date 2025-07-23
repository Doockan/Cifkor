using UniRx;
using Zenject;

namespace Assets.Scripts.Features.MainTabs
{
    public class MainTabsController : IInitializable
    {
        private readonly MainTabsView _mainTabsView;
        public readonly Subject<ETabType> OnTabChangeAction = new Subject<ETabType>();

        public MainTabsController(MainTabsView mainTabsView)
        {
            _mainTabsView = mainTabsView;
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
            OnTabChangeAction.OnNext(tab);
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