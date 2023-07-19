using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///    This script allows customizing the properties of a Light component, such as brightness, color, and temperature.
/// </summary>
/// <author> Collin June </author>
public class CustomLightScript : MonoBehaviour
{
    private Light lightComponent;

    private void Start()
    {
        // Get the Light component attached to the same GameObject
        lightComponent = GetComponent<Light>();
    }

    /// <summary>
    ///    Sets the brightness (intensity) of the Light component.
    /// </summary>
    /// <param name="brightness"> The brightness value to be set. </param>
    public void SetBrightness(float brightness)
    {
        // Set the intensity of the Light component based on the provided brightness value
        lightComponent.intensity = brightness;
    }

    /// <summary>
    ///    Sets the color of the Light component.
    /// </summary>
    /// <param name="color"> The color value to be set. </param>
    public void SetColor(Color color)
    {
        // Set the color of the Light component based on the provided color value
        lightComponent.color = color;
    }

    /// <summary>
    ///    Sets the color temperature of the Light component.
    /// </summary>
    /// <param name="temperature"> The color temperature value to be set. </param>
    public void SetTemperature(float temperature)
    {
        // Set the color temperature of the Light component based on the provided temperature value
        lightComponent.colorTemperature = temperature;
    }
}
