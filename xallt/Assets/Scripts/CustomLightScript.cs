using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLightScript : MonoBehaviour
{
    private Light lightComponent;

    private void Start()
    {
        // Get the Light component attached to the same GameObject
        lightComponent = GetComponent<Light>();
    }

    public void SetBrightness(float brightness)
    {
        // Set the intensity of the Light component based on the provided brightness value
        lightComponent.intensity = brightness;
    }

    public void SetColor(Color color)
    {
        // Set the color of the Light component based on the provided color value
        lightComponent.color = color;
    }

    public void SetTemperature(float temperature)
    {
        // Set the color temperature of the Light component based on the provided temperature value
        lightComponent.colorTemperature = temperature;
    }
}