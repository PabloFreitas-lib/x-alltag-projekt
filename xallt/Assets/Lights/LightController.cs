using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private CustomLightScript[] lights;

    public bool useTemperatureControl = true; // Toggle between temperature and RGB control
    public float brightness = 1f;
    public Color color = Color.white;
    public float temperature = 6500f;

    private void Start()
    {
        // Find all light components with the CustomLightScript attached
        lights = FindObjectsOfType<CustomLightScript>();
    }

    public void SetBrightness(float brightness)
    {
        foreach (CustomLightScript light in lights)
        {
            light.SetBrightness(brightness);
        }
    }

    public void SetColor(Color color)
    {
        foreach (CustomLightScript light in lights)
        {
            light.SetColor(color);
        }
    }

    public void SetTemperature(float temperature)
    {
        foreach (CustomLightScript light in lights)
        {
            light.SetTemperature(temperature);
        }
    }

    private void Update()
    {
        // Update the properties of all lights every frame
        foreach (CustomLightScript light in lights)
        {
            if (useTemperatureControl)
            {
                light.SetColor(Color.white);
                light.SetTemperature(temperature);
                light.SetBrightness(brightness);
            }
            else
            {
                light.SetTemperature(0);
                light.SetColor(color);
                light.SetBrightness(brightness);
            }
        }
    }
}
