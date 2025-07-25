using UnityEngine;
using UnityEngine.UIElements;

public class SpinnerController : MonoBehaviour
{
    public VisualElement spinner;
    private float angle = 0;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        spinner = root.Q<VisualElement>("Spinner");
    }

    void Update()
    {
        if (spinner != null)
        {
            angle += 360 * Time.deltaTime / 0.8f;
            spinner.style.rotate = new StyleRotate(new Rotate(angle));
        }
    }
}