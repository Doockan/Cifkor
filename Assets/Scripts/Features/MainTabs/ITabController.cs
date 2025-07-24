namespace Assets.Scripts.Features.MainTabs
{
    public interface ITabController
    {
        ETabType TabType { get; }
        void ActivateTab();
        void DeactivateTab();
    }
}