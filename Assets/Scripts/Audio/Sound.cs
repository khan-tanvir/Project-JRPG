using UnityEngine;

public enum SoundType
{
    Music = 0,
    Effect = 1,
    UI = 2
}

[System.Serializable]
public class Sound
{
    [SerializeField]
    private AudioClip _clip;

    [SerializeField]
    private string _name;

    [SerializeField]
    private SoundType _type;

    [Range(0.0f, 1.0f)]
    public float volume;

    [HideInInspector]
    public AudioSource source;

    // Reference to the audio file
    public AudioClip Clip
    {
        get { return _clip; }
    }

    // Variables of the sound file
    public string Name
    {
        get { return _name; }
    }

    public SoundType Type
    {
        get { return _type; }
    }
}