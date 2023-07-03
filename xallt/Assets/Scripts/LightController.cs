using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private CustomLightScript[] lights;

    public bool useTemperatureControl = true; // Toggle between temperature and RGB control
    public float brightness = 1f;
    public Color color = new Color(255f/255f, 180f/255f, 107f/255f);

    // Slider for light temperature / color change
    [Range(0f, 130f)]
    public float tempSlider;

    public bool activateAllLights = true; // Toggle to activate or disable all lights

    private void Start()
    {
        // Find all light components with the CustomLightScript attached
        lights = FindObjectsOfType<CustomLightScript>();
        // Starting point on slider
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

    private void Update()
    {
        if (activateAllLights)
        {
            // Activate all lights
            foreach (CustomLightScript light in lights)
            {
                light.gameObject.SetActive(true);
            }
        }
        else
        {
            // Disable all lights
            foreach (CustomLightScript light in lights)
            {
                light.gameObject.SetActive(false);
            }
        }

        Color currentColor;
        if (useTemperatureControl)
        {
            currentColor = new Color(1f, ((169f + (tempSlider/2))/255), ((87 + tempSlider)/255));
        }
        else
        {
            currentColor = new Color(color.r, color.g, color.b);
        }

        SetColor(currentColor);
        SetBrightness(brightness);
    }
}
