using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for additional day-night modules.
/// </summary>
/// <author> Collin June </author>
public abstract class DNModuleBase : MonoBehaviour
{
    protected DayNightController dayNightControl; // Reference to the DayNightController

    private void OnEnable()
    {
        dayNightControl = this.GetComponent<DayNightController>();
        if (dayNightControl != null)
            dayNightControl.AddModule(this);
    }

    private void OnDisable()
    {
        if (dayNightControl != null)
            dayNightControl.RemoveModule(this);
    }

    /// <summary>
    /// Updates the module based on the current sun intensity.
    /// This method must be implemented by the derived class.
    /// </summary>
    /// <param name="intensity">The current intensity of the sun.</param>
    public abstract void UpdateModule(float intensity);
}
