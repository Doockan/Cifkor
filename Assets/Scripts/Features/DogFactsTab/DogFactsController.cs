using UnityEngine.UIElements;

namespace Assets.Scripts.Features.DogFactsTab
{
    public class DogFactsController
    {
        private readonly DogFactsView _view;

        public DogFactsController(DogFactsView dogFactsView)
        {
            _view = dogFactsView;
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
