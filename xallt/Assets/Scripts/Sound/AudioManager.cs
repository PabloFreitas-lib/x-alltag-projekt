using System;
using UnityEngine.Audio;
using UnityEngine;

/// <summary>
/// Maps statements like "Sound with name X and properties Y should be played" to the UnityEngine.Audio logic.
/// Additional sounds may be added through the editor.
/// </summary>
/// <author>Celina Dadschun, Laura Gietschel, Sophia Gommeringer, Jakob Kern, Norman Köhler, Minoush Prieb, Pablo Santos</author>
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    /// <summary>
    /// Creates an audio source for every sound in the editor defined sounds list on launch.
    /// </summary>
    /// <author>Jakob Kern</author>
    void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;
        }
    }

    /// <summary>
    /// Plays a sound with a given name, if it exists and is not already playing.
    /// </summary>
    /// <param name="name">The name of the sound to be played.</param>
    /// <author>Jakob Kern</author>
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null || (s.source.isPlaying))
        {
            Debug.Log(name + " is not an appropriate sound name or it is already being played");
            return;
        }
        s.source.Play();
    }

    /// <summary>
    /// Stops a sound with a given name, if it exists and only if it is already playing.
    /// </summary>
    /// <param name="name">The name of the sound to be played.</param>
    /// <author>Jakob Kern</author>
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || !(s.source.isPlaying))
        {
            Debug.Log(name + " is not an appropriate sound name or the correlating sound is not being played.");
            return;
        }
        s.source.Stop();
    }
}
