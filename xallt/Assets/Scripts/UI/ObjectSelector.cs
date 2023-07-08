using UnityEngine;
using UnityEngine.UI;

public class ObjectSelector : MonoBehaviour
{
    public Dropdown dropdown;
    public GameObject[] objectsToSelect;

    private void Start()
    {
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void OnDropdownValueChanged(int index)
    {
        if (index >= 0 && index < objectsToSelect.Length)
        {
            GameObject selectedObject = objectsToSelect[index];
            // Handle the selected object, for example, by instantiating it in the scene.
            Instantiate(selectedObject);
        }
    }
}