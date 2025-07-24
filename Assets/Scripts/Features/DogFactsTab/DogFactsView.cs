using UnityEngine;
using UnityEngine.UIElements;
using UniRx;
using System;
using System.Collections.Generic;
using Assets.Scripts.Core.Utils;
using static Assets.Scripts.Core.Utils.DogBreedDataModel;

namespace Assets.Scripts.Features.DogFactsTab
{
    public class DogFactsView : MonoBehaviour
    {
        [SerializeField] private UIDocument _uIDocument;

        public VisualElement Root => _uIDocument.rootVisualElement.Q<VisualElement>("DogTab");
        public ListView DogBreedsList => Root.Q<ListView>("DogBreedsList");
        public VisualElement FactPopup => Root.Q<VisualElement>("DogFactPopup");
        public Label FactTitle => FactPopup.Q<Label>("DogFactTitle");
        public Label FactDescription => FactPopup.Q<Label>("DogFactDescription");
        public Button FactCloseButton => FactPopup.Q<Button>("DogFactCloseButton");
        public VisualElement Loader => Root.Q<VisualElement>("Loader"); // если есть отдельный loader
        public VisualElement FactLoader => FactPopup.Q<VisualElement>("FactLoader"); // если есть

        private Subject<string> _breedClicked = new Subject<string>();
        public IObservable<string> OnBreedClicked => _breedClicked;

        private List<BreedData> _breeds;

        public void ShowLoader(bool show)
        {
            if (Loader != null)
                Loader.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }

        public void ShowFactLoader(bool show)
        {
            if (FactLoader != null)
                FactLoader.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }

        public void HideBreedFactPopup()
        {
            FactPopup.style.display = DisplayStyle.None;
        }

        public void ShowBreedFactPopup(string title, string description)
        {
            FactTitle.text = title;
            FactDescription.text = description;
            FactPopup.style.display = DisplayStyle.Flex;
        }

        public void SetBreedsList(DogBreedDataListModel breedsModel)
        {
            _breeds = breedsModel.Data;
            DogBreedsList.itemsSource = _breeds;
            DogBreedsList.makeItem = () => new Label();
            DogBreedsList.bindItem = (element, i) =>
            {
                var breed = _breeds[i];
                (element as Label).text = $"{i + 1} - {breed.Attributes.Name}";
            };
            DogBreedsList.selectionType = SelectionType.Single;
            DogBreedsList.onSelectionChange += OnBreedSelected;
        }

        private void OnBreedSelected(IEnumerable<object> selected)
        {
            foreach (var sel in selected)
            {
                if (sel is BreedData breed)
                {
                    _breedClicked.OnNext(breed.Id);
                }
            }

            DogBreedsList.ClearSelection();
        }

        private void Awake()
        {
            FactCloseButton.clicked += HideBreedFactPopup;
        }
    }
}