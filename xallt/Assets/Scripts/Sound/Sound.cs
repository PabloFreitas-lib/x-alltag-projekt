using UnityEngine.Audio;
using UnityEngine;


/// <summary>
/// Encapsulates a sound with its relevant settings.
/// Used by Audio Manager to play a sound with its predefined (hard-coded) options.
/// Values of instances may be set through the editor.
/// </summary>
[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f,1f)]
    public float volume;
    [Range(0f, 1f)]
    public float spatialBlend;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
