using InfoGamerHubAssets;
using UnityEngine;
using UnityEngine.UI;

public class LightController : MonoBehaviour
{
    private CustomLightScript[] lights;

    public bool useTemperatureControl = true; // Toggle between temperature and RGB control
    public float brightness = 1f;
    public Color color = new Color(255f / 255f, 180f / 255f, 107f / 255f);
    public UIColorPickButton colorPickButton;
    public Slider brightnessSlider; // Reference to the UI Slider for brightness control
    public Toggle activateAllLightsToggle; // Reference to the UI Toggle for activating or disabling all lights
    public bool activateAllLights;

    // Slider for light temperature / color change
    [Range(0f, 130f)]
    public float tempSlider;

    private void Start()
    {
        // Find all light components with the CustomLightScript attached
        lights = FindObjectsOfType<CustomLightScript>();
        // Starting point on slider
        tempSlider = 0f;

        // Subscribe to the ColorPickerEvent
        colorPickButton.ColorPickerEvent.AddListener(OnColorPicked);

        // Add listeners to the UI Slider and Toggle
        brightnessSlider.onValueChanged.AddListener(OnBrightnessSliderValueChanged);
        activateAllLightsToggle.onValueChanged.AddListener(OnActivateAllLightsToggleValueChanged);
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

    // Event listener for ColorPickerEvent
    private void OnColorPicked(Color pickedColor)
    {
        color = pickedColor;
    }

    // Event listener for brightness slider value changed
    private void OnBrightnessSliderValueChanged(float value)
    {
        brightness = value;
        SetBrightness(brightness);
    }

    // Event listener for activate all lights toggle value changed
    private void OnActivateAllLightsToggleValueChanged(bool value)
    {
        activateAllLights = value;
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
            currentColor = new Color(1f, ((169f + (tempSlider / 2)) / 255), ((87 + tempSlider) / 255));
        }
        else
        {
            currentColor = colorPickButton.newcolor;
        }

        SetColor(currentColor);
        SetBrightness(brightness);
    }
}

