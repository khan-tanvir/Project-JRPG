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
    #region Private Fields

    [SerializeField]
    private AudioClip _clip;

    [SerializeField]
    private string _name;

    [SerializeField]
    private SoundType _type;

    #endregion Private Fields

    #region Public Fields

    [HideInInspector]
    public AudioSource source;

    [Range(0.0f, 1.0f)]
    public float volume;

    #endregion Public Fields

    #region Public Properties

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

    #endregion Public Properties
}