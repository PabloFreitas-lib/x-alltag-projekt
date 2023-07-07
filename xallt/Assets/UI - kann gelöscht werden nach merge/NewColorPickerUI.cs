using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NewColorPickerUI : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public Image colorWheelImage;
    public float hue;
    public float saturation;
    public float value;

    private RectTransform colorWheelRectTransform;
    private Vector2 colorWheelCenter;
    private float colorWheelRadius;

    private void Start()
    {
        // Get the RectTransform of the color wheel image
        colorWheelRectTransform = colorWheelImage.GetComponent<RectTransform>();

        // Calculate the center and radius of the color wheel
        colorWheelCenter = colorWheelRectTransform.position;
        colorWheelRadius = colorWheelRectTransform.rect.width / 2f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Call the color picking function when a pointer is initially pressed
        PickColor(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Call the color picking function as the pointer is dragged
        PickColor(eventData.position);
    }

    private void PickColor(Vector2 position)
    {
        // Calculate the distance from the center of the color wheel
        Vector2 positionRelativeToCenter = position - colorWheelCenter;
        float distanceFromCenter = positionRelativeToCenter.magnitude;

        // Check if the position is within the color wheel radius
        if (distanceFromCenter <= colorWheelRadius)
        {
            // Calculate the angle of the position
            float angle = Mathf.Atan2(positionRelativeToCenter.y, positionRelativeToCenter.x);
            if (angle < 0f)
            {
                angle += Mathf.PI * 2f; // 
            }

            // Calculate the hue based on the angle
            hue = angle * Mathf.Rad2Deg / 360f;

            // Calculate the saturation and value based on the distance from the center
            saturation = distanceFromCenter / colorWheelRadius;
            value = 1f;

            // Set the color of the color wheel image to the picked color
            colorWheelImage.color = Color.HSVToRGB(hue, saturation, value);

            // Use the picked color for your desired purposes (e.g., change material color, etc.)
            // ...
        }
    }
}