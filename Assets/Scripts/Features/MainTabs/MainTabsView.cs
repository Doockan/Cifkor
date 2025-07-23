using UnityEngine;
using UnityEngine.UIElements;

public class MainTabsView : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    public Button TabClicker => _uiDocument.rootVisualElement.Q<Button>("TabClicker");
    public Button TabWeather => _uiDocument.rootVisualElement.Q<Button>("TabWeather");
    public Button TabDog => _uiDocument.rootVisualElement.Q<Button>("TabDog");
}