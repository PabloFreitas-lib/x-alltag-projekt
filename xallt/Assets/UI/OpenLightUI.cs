using UnityEngine;

public class OpenLightUI : MonoBehaviour
{
    public Canvas canvas;
    private bool isCanvasEnabled;

    private void Start()
    {
        isCanvasEnabled = false;
        canvas.enabled = isCanvasEnabled;
    }

    private void OnMouseDown()
    {
        isCanvasEnabled = !isCanvasEnabled;
        canvas.enabled = isCanvasEnabled;
    }
}