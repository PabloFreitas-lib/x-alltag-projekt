using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A day-night module that updates the skybox colors based on the current sun intensity.
/// </summary>
/// <author> Collin June </author>
public class SkyBoxModule : DNModuleBase
{
    [SerializeField]
    private Gradient skyColor; // Gradient representing the sky color at different sun intensities

    [SerializeField]
    private Gradient horizonColor; // Gradient representing the horizon color at different sun intensities

    /// <summary>
    /// Updates the skybox colors based on the current sun intensity.
    /// </summary>
    /// <param name="intensity">The current intensity of the sun.</param>
    public override void UpdateModule(float intensity)
    {
        // Evaluate the sky color gradient based on the current sun intensity and update the sky tint in the skybox
        RenderSettings.skybox.SetColor("_SkyTint", skyColor.Evaluate(intensity));

        // Evaluate the horizon color gradient based on the current sun intensity and update the ground color in the skybox
        RenderSettings.skybox.SetColor("_GroundColor", horizonColor.Evaluate(intensity));
    }
}
