using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

/// <summary>
/// Controls the day and night cycle, including sun animation and intensity.
/// </summary>
/// <author> Collin June </author>
public class DayNightController : MonoBehaviour
{
    [SerializeField] private Animator lightAnimator; // Reference to the light Animator component
    [SerializeField] private AnimationClip sunAnimationClip; // Reference to the sun animation clip

    [Header("Sun")]
    public Light sun; // Reference to the sun's light component
    public float intensity; // Intensity of the sun

    [Header("Sun Animation")]
    public bool useTestAnimation = false; // Set to true to use test animation
    public float testAnimationSpeed = 1f; // Speed of test animation

    [Header("Sun Test Time")]
    public bool useTestTime = false; // Set to true to use test animation
    public int testAnimationHours;
    public int testAnimationMinutes;
    public int testAnimationSeconds;

    private float timeOfDay; // The current time of day in decimal format (hours.minutes)

    [Header("Modules")]
    private List<DNModuleBase> moduleList = new List<DNModuleBase>(); // List to store additional day-night modules

    private void Update()
    {
        UpdateTimeOfDay(); // Update the current time of day
        UpdateSunIntensity(); // Update the intensity of the sun based on its rotation
        UpdateModules(); // Update any additional day-night modules

        if (useTestTime)
        {
            AdjustTestSunTime(testAnimationHours, testAnimationMinutes, testAnimationSeconds); // Adjust sun animation using test time
        }
        else if (useTestAnimation)
        {
            AdjustTestSunAnimation(); // Adjust sun animation using test animation speed
        }
        else
        {
            AdjustSunAnimation(); // Adjust sun animation based on the current time of day
        }
    }

    /// <summary>
    /// Updates the current time of day based on the system clock or test time.
    /// </summary>
    private void UpdateTimeOfDay()
    {
        if (!useTestAnimation)
        {
            System.DateTime currentTime = System.DateTime.Now;
            float hours = currentTime.Hour;
            float minutes = currentTime.Minute;
            float seconds = currentTime.Second;

            // Calculate the current time of day in decimal format (hours.minutes)
            timeOfDay = hours + minutes / 60f + seconds / 3600f;
        }
    }

    /// <summary>
    /// Updates the intensity of the sun based on its rotation and adjusts shadow strength accordingly.
    /// </summary>
    private void UpdateSunIntensity()
    {
        float sunRotationX = sun.transform.localRotation.eulerAngles.x;

        // Calculate the target intensity based on the sun's rotation angle
        float targetIntensity = Mathf.InverseLerp(-15f, 20f, sunRotationX);

        // Set intensity to 0 if the sun is below the horizon (sunRotationX < 0 or sunRotationX > 180)
        if (sunRotationX < 0f || sunRotationX > 180f)
        {
            targetIntensity = 0f;
        }

        // Smoothly adjust the intensity towards the target intensity
        intensity = Mathf.Lerp(intensity, targetIntensity, Time.deltaTime);

        // Adjust shadow strength based on the sun's intensity
        sun.shadowStrength = intensity;
    }

    /// <summary>
    /// Adjusts the sun animation based on the current time of day.
    /// </summary>
    private void AdjustSunAnimation()
    {
        // Calculate the normalized animation time based on the current time of day
        float animationTime = CalculateAnimationTime();

        // Play the sun animation at the calculated animation time
        PlaySunAnimation(animationTime);
    }

    /// <summary>
    /// Adjusts the sun animation based on the provided test time.
    /// </summary>
    /// <param name="hours">The hours of the test time.</param>
    /// <param name="minutes">The minutes of the test time.</param>
    /// <param name="seconds">The seconds of the test time.</param>
    private void AdjustTestSunTime(int hours, int minutes, int seconds)
    {
        // Calculate the normalized animation time based on the test time provided
        float animationTime = CalculateTestAnimationTime(hours, minutes, seconds);

        // Play the sun animation at the calculated animation time
        PlaySunAnimation(animationTime);
    }

    /// <summary>
    /// Plays the sun animation at the given animation time.
    /// </summary>
    /// <param name="animationTime">The normalized animation time (0 to 1).</param>
    private void PlaySunAnimation(float animationTime)
    {
        // Set the "Sunposition" parameter of the light animator to the given animation time
        lightAnimator.SetFloat("Sunposition", animationTime);

        // Play the sun animation using the provided sun animation clip
        lightAnimator.Play(sunAnimationClip.name);
    }

    /// <summary>
    /// Calculates the normalized animation time based on the current time of day for the sun animation.
    /// </summary>
    /// <returns>The normalized animation time (0 to 1).</returns>
    private float CalculateAnimationTime()
    {
        float animationTime = 0f;

        // Define the time of sunrise (in hours) - adjust this value based on your scene's sunrise time
        float sunriseTime = 5f;

        // Calculate the total time of day in seconds
        float totalTimeInSeconds = timeOfDay * 3600f;

        // Calculate the time since sunrise in seconds
        float timeSinceSunrise = totalTimeInSeconds - (sunriseTime * 3600f);

        // Calculate the normalized animation time (0 to 1) based on the time since sunrise
        animationTime = timeSinceSunrise / (24f * 3600f);
        animationTime = Mathf.Repeat(animationTime, 1f);

        return animationTime;
    }

    /// <summary>
    /// Calculates the normalized animation time based on the provided test time for the sun animation.
    /// </summary>
    /// <param name="hours">The hours of the test time.</param>
    /// <param name="minutes">The minutes of the test time.</param>
    /// <param name="seconds">The seconds of the test time.</param>
    /// <returns>The normalized animation time (0 to 1).</returns>
    private float CalculateTestAnimationTime(int hours, int minutes, int seconds)
    {
        // Define the time of sunrise (in hours) - adjust this value based on your scene's sunrise time
        float sunriseTime = 5f;

        // Calculate the total time of day in seconds based on the test time provided
        float totalTimeInSeconds = hours * 3600f + minutes * 60f + seconds;

        // Calculate the normalized animation time (0 to 1) based on the time since sunrise
        float timeSinceSunrise = totalTimeInSeconds - (sunriseTime * 3600f);
        float animationTime = timeSinceSunrise / (24f * 3600f);
        animationTime = Mathf.Repeat(animationTime, 1f);

        return animationTime;
    }

    /// <summary>
    /// Adjusts the sun animation using the test animation speed.
    /// </summary>
    private void AdjustTestSunAnimation()
    {
        // Convert the testAnimationSpeed to animation speed (0 to 1) based on 24 hours
        float animationSpeed = testAnimationSpeed / 24f;

        // Calculate the new animation time based on the current animation time and the animation speed
        float animationTime = lightAnimator.GetFloat("Sunposition") + (animationSpeed * Time.deltaTime);

        // Set the "Sunposition" parameter of the light animator to the new animation time
        lightAnimator.SetFloat("Sunposition", animationTime);

        // Play the sun animation using the provided sun animation clip
        lightAnimator.Play(sunAnimationClip.name);
    }

    /// <summary>
    /// Adds a new day-night module to the controller.
    /// </summary>
    /// <param name="module">The day-night module to add.</param>
    public void AddModule(DNModuleBase module)
    {
        moduleList.Add(module);
    }

    /// <summary>
    /// Removes a day-night module from the controller.
    /// </summary>
    /// <param name="module">The day-night module to remove.</param>
    public void RemoveModule(DNModuleBase module)
    {
        moduleList.Remove(module);
    }

    /// <summary>
    /// Updates all additional day-night modules with the current sun intensity.
    /// </summary>
    private void UpdateModules()
    {
        // Update each additional day-night module with the current sun intensity
        foreach (DNModuleBase module in moduleList)
        {
            module.UpdateModule(intensity);
        }
    }
}
