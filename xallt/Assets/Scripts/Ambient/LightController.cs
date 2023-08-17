using InfoGamerHubAssets;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the behavior of multiple custom lights in the scene, allowing manual control of brightness and color.
/// </summary>
/// <author> Collin June, Christoph Dreier </author>
public class LightController : MonoBehaviour
{
    private CustomLightScript[] lights;

    private CustomEmissionScript[] emissions;

    [Header("UI Components")]
    public UIColorPickButton colorPickButton; // Color picker button for selecting a color
    public Slider brightnessSlider; // UI Slider for brightness control
    public Toggle activateAllLightsToggle; // UI Toggle for activating or disabling all lights
    public Toggle activateTempToggle; // UI Toggle for switching between temperature and RGB control
    public Slider temperatureSlider; // Slider for light temperature / color change

    [Header("Manual Control")]
    public bool useTemperatureControl; // Toggle between temperature and RGB control
    public float brightness = 1f; // Current brightness value
    public Color color = new Color(255f / 255f, 180f / 255f, 107f / 255f); // Current color value
    public bool activateAllLights; // Toggle for activating or disabling all lights

    // Slider for light temperature / color change
    [Range(0f, 130f)]
    public float tempSlider; // Current value of the temperature slider

    private void Start()
    {
        // Find all light components with the CustomLightScript attached
        lights = FindObjectsOfType<CustomLightScript>();

        // Find all material components with the CustomEmissionScript attached
        emissions = FindObjectsOfType<CustomEmissionScript>();

        // Starting point on slider
        tempSlider = 0f;

        // Subscribe to the ColorPickerEvent
        colorPickButton.ColorPickerEvent.AddListener(OnColorPicked);

        // Add listeners to the UI Slider and Toggle
        brightnessSlider.onValueChanged.AddListener(OnBrightnessSliderValueChanged);
        activateAllLightsToggle.onValueChanged.AddListener(OnActivateAllLightsToggleValueChanged);
        temperatureSlider.onValueChanged.AddListener(OnTemperatureSliderValueChanged);
        activateTempToggle.onValueChanged.AddListener(OnActivateTemperatureControlToggleValueChanged);
    }

    /// <summary>
    /// Sets the brightness for all lights in the scene.
    /// </summary>
    /// <author> Collin June, Christoph Dreier </author>
    /// <param name="brightness"> The new brightness value. </param>
    public void SetBrightness(float brightness)
    {
        foreach (CustomLightScript light in lights)
        {
            light.SetBrightness(brightness);
        }
    }

    /// <summary>
    /// Sets the color for all lights in the scene.
    /// </summary>
    /// <author> Collin June, Christoph Dreier </author>
    /// <param name="color"> The new color value. </param>
    public void SetColor(Color color)
    {
        foreach (CustomLightScript light in lights)
        {
            light.SetColor(color);
        }
    }

    public void SetEmissionColor(Color color)
    {
        foreach (CustomEmissionScript emission in emissions)
        {
            emission.SetEmissionColor(color);
        }
    }

    /// <summary>
    /// Event listener for the ColorPickerEvent.
    /// </summary>
    /// <author> Collin June, Christoph Dreier </author>
    /// <param name="pickedColor"> The color picked by the color picker. </param>
    private void OnColorPicked(Color pickedColor)
    {
        color = pickedColor; // Update the current color with the picked color
    }

    /// <summary>
    /// Event listener for brightness slider value changed.
    /// </summary>
    /// <author> Collin June, Christoph Dreier </author>
    /// <param name="value"> The new value of the brightness slider. </param>
    private void OnBrightnessSliderValueChanged(float value)
    {
        brightness = value; // Update the current brightness with the slider value
        SetBrightness(brightness); // Set the new brightness for all lights
    }

    /// <summary>
    /// Event listener for temperature slider value changed.
    /// </summary>
    /// <author> Collin June, Christoph Dreier </author>
    /// <param name="value"> The new value of the temperature slider. </param>
    private void OnTemperatureSliderValueChanged(float value)
    {
        tempSlider = value; // Update the current temperature slider value
    }

    /// <summary>
    /// Event listener for activate all lights toggle value changed.
    /// </summary>
    /// <author> Collin June, Christoph Dreier </author>
    /// <param name="value"> The new value of the activate all lights toggle. </param>
    private void OnActivateAllLightsToggleValueChanged(bool value)
    {
        activateAllLights = value; // Update the activateAllLights toggle
    }

    /// <summary>
    /// Event listener for activate temperature control toggle value changed.
    /// </summary>
    /// <author> Collin June, Christoph Dreier </author>
    /// <param name="value"> The new value of the activate temperature control toggle. </param>
    private void OnActivateTemperatureControlToggleValueChanged(bool value)
    {
        useTemperatureControl = value; // Update the useTemperatureControl toggle
    }

    /// <summary>
    /// updating the light objects and logic for changing the lights.
    /// </summary>
    /// <author> Christoph Dreier </author>
    private void Update()
    {

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
        SetEmissionColor(currentColor);

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

            SetEmissionColor(Color.black);
        }
    }
}
