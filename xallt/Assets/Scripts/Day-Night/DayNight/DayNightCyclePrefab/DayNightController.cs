using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightController : MonoBehaviour
{
    [SerializeField] private Animator lightAnimator; // Reference to the light Animator component
    [SerializeField] private AnimationClip sunAnimationClip; // Reference to the sun animation clip

    [Header("Sun")]
    public Light sun; // Reference to the sun's light component
    public float intensity; // Intensity of the sun
    public float sunIntensityScale;

    [Header("Sun Animation")]
    public bool useTestAnimation = false; // Set to true to use test animation
    public float testAnimationSpeed = 1f; // Speed of test animation

    [Header("Sun Test Time")]
    public bool useTestTime = false; // Set to true to use test animation
    public int testAnimationHours;
    public int testAnimationMinutes;
    public int testAnimationSeconds;

    private float timeOfDay;

    [Header("Modules")]
    private List<DNModuleBase> moduleList = new List<DNModuleBase>();

    private void Update()
    {
        UpdateTimeOfDay();
        UpdateSunIntensity();
        UpdateModules();

        if (useTestTime)
        {
            AdjustTestSunTime(testAnimationHours, testAnimationMinutes, testAnimationSeconds);
        }
        else if (useTestAnimation)
        {
            AdjustTestSunAnimation();
        }
        else
        {
            AdjustSunAnimation();
        }
    }

    private void UpdateTimeOfDay()
    {
        if (!useTestAnimation)
        {
            System.DateTime currentTime = System.DateTime.Now;
            float hours = currentTime.Hour;
            float minutes = currentTime.Minute;
            float seconds = currentTime.Second;

            timeOfDay = hours + minutes / 60f + seconds / 3600f;
        }
    }

    private void UpdateSunIntensity()
    {
        float sunRotationX = sun.transform.localRotation.eulerAngles.x;
        intensity = Mathf.InverseLerp(-15f, 20f, sunRotationX);
    }

    private void AdjustSunAnimation()
    {
        float animationTime = CalculateAnimationTime();
        PlaySunAnimation(animationTime);
    }

    private void AdjustTestSunTime(int hours, int minutes, int seconds)
    {
        float animationTime = CalculateTestAnimationTime(hours, minutes, seconds);
        PlaySunAnimation(animationTime);
    }

    private void PlaySunAnimation(float animationTime)
    {
        lightAnimator.SetFloat("Sunposition", animationTime);
        lightAnimator.Play(sunAnimationClip.name);
    }

    private float CalculateAnimationTime()
    {
        float animationTime = 0f;

        float sunriseTime = 5f;
        float totalTimeInSeconds = timeOfDay * 3600f;
        float timeSinceSunrise = totalTimeInSeconds - (sunriseTime * 3600f);
        animationTime = timeSinceSunrise / (24f * 3600f);
        animationTime = Mathf.Repeat(animationTime, 1f);

        return animationTime;
    }

    private float CalculateTestAnimationTime(int hours, int minutes, int seconds)
    {
        float sunriseTime = 5f;
        float totalTimeInSeconds = hours * 3600f + minutes * 60f + seconds;
        float animationTime = totalTimeInSeconds / (24f * 3600f);
        float timeSinceSunrise = totalTimeInSeconds - (sunriseTime * 3600f);
        animationTime = timeSinceSunrise / (24f * 3600f);
        animationTime = Mathf.Repeat(animationTime, 1f);

        return animationTime;
    }

    private void AdjustTestSunAnimation()
    {
        float animationSpeed = testAnimationSpeed / 24f; // Convert testAnimationSpeed to animation speed
        float animationTime = lightAnimator.GetFloat("Sunposition") + (animationSpeed * Time.deltaTime);
        lightAnimator.SetFloat("Sunposition", animationTime);
        lightAnimator.Play(sunAnimationClip.name);
    }

    public void AddModule(DNModuleBase module)
    {
        moduleList.Add(module);
    }

    public void RemoveModule(DNModuleBase module)
    {
        moduleList.Remove(module);
    }

    private void UpdateModules()
    {
        foreach (DNModuleBase module in moduleList)
        {
            module.UpdateModule(intensity);
        }
    }
}