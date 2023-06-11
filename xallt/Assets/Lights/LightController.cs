using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private CustomLightScript[] lights;

    public bool useTemperatureControl = true; // Toggle between temperature and RGB control
    public bool lightonoff = true;  //lightswitch
    public float brightness = 1f;
    public Color color = Color.HSVToRGB(60f / 360f, 1f, 1f);
    public float temperature;


    // Slider for light temperature / color change
    [Range(0f,1f)]
    public float tempSlider;

    private void Start()
    {
        // Find all light components with the CustomLightScript attached
        lights = FindObjectsOfType<CustomLightScript>();
        // Startingpoint in Slider
        tempSlider = 0f;
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
                Color color = Color.HSVToRGB(45f / 360f, tempSlider, 1f); // light temperature
                light.SetColor(color);
                light.SetTemperature(temperature);
                light.SetBrightness(brightness);
            }
            else
            {
                Color color = Color.HSVToRGB(tempSlider, 1f, 1f); // color change 
                light.SetTemperature(0);
                light.SetColor(color);
                light.SetBrightness(brightness);
            }

            if (lightonoff)
            {
                //lights on
                if (useTemperatureControl)
                {
                    Color color = Color.HSVToRGB(45f / 360f, tempSlider, 1f);
                    light.SetColor(color);
                }
                else
                {
                    Color color = Color.HSVToRGB(tempSlider, 1f, 1f);
                    light.SetColor(color);
                }
            }
            else
            {
                //lights off
                if (useTemperatureControl)
                {
                    Color color = Color.HSVToRGB(45f / 360f, tempSlider, 0f); // light temperature
                    light.SetColor(color);
                }
                else
                {
                    Color color = Color.HSVToRGB(tempSlider, 1f, 0f);
                    light.SetColor(color);
                }

            }
        }
    }
}
