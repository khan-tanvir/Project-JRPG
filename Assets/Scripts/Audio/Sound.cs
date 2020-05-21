using UnityEngine;

public enum SoundType
{
    Music,
    Effect,
    UI
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

    [HideInInspector]
    public AudioSource source;
}