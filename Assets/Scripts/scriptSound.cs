using UnityEngine;

[System.Serializable]
public class scriptSound
{
    // Reference to the audio file
    public AudioClip clip;

    // Variables of the sound file
    public string name;

    [Range(0.0f, 1.0f)]
    public float volume;

    public string audioType;

    public bool mute;

    [HideInInspector]
    public AudioSource source;
}