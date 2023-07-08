using UnityEngine;

public class OpenLightUI : MonoBehaviour
{
    public Canvas canvas;
    private bool isCanvasEnabled;

    private void Start()
    {
        canvas.gameObject.SetActive(false);
        //canvas.enabled = isCanvasEnabled;
    }

    private void OnMouseDown()
    {
        //isCanvasEnabled = !isCanvasEnabled;
        //canvas.enabled = isCanvasEnabled;
    }
    public void UIsetActive()
    {
        gameObject.SetActive(!gameObject.active);
    }
}