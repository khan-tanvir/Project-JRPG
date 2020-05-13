using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class scriptAudioManager : MonoBehaviour
{
    // TODO: Fade-In and Fade-Out
    // TODO: Sound Effects
    
    public AudioMixer audioMixer;

    // Store which music is playing
    [SerializeField]
    private string currentMusic;

    [SerializeField]
    private Slider _masterSlider;

    [SerializeField]
    private Slider _musicSlider;

    // Hold reference to this object
    public static scriptAudioManager audioManager;

    // Array that stores all sounds
    public scriptSound[] sounds;

    public bool _isInitialised = false;

    private void Awake()
    {
        if (audioManager == null)
        {
            DontDestroyOnLoad(gameObject);
            audioManager = this;
        }
        else if (audioManager != null)
            Destroy(gameObject);

        foreach (scriptSound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.outputAudioMixerGroup = audioMixer.FindMatchingGroups(sound.audioType)[0];
            sound.source.clip = sound.clip;
            sound.source.name = sound.name;
            sound.source.volume = sound.volume;
            sound.source.mute = sound.mute;
        }

        // Countering a bug that changes the name to the 2nd element of sounds
        gameObject.name = "AudioManager";

    }

    public void Start()
    {
        _masterSlider.value = scriptGameData.gameData.MasterVolume;
        _musicSlider.value = scriptGameData.gameData.MusicVolume;
    }

    public void SetMasterVolume(float volume)
    {
        // Using log10 for a better slider
        audioMixer.SetFloat("Master Volume", Mathf.Log10(volume) * 20.0f);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music Volume", Mathf.Log10(volume) * 20.0f);
    }

    public scriptSound Find(string name)
    {
        scriptSound soundToFind = Array.Find(sounds, clip => clip.name == name);
        return soundToFind;
    }

    public void PlayMusic(string name)
    {
        if (!IsPlaying(name))
        {
            // Stop the current music before playing requested music
            if (!string.IsNullOrEmpty(currentMusic))
                StopMusic(currentMusic);

            Find(name).source.Play();
            currentMusic = name;
        }
    }

    public bool IsPlaying(string name)
    {
        if (Find(name).source.isPlaying)
        {
            return true;
        }
        else
            return false;
    }

    public void EnableMusicLoop()
    {
        Find(currentMusic).source.loop = true;
    }

    public void DisableMusicLoop()
    {
        Find(currentMusic).source.loop = false;
    }

    public void StopMusic(string name)
    {
        if (Find(name).source != null)
        {
            if (Find(name).source.isPlaying)
            {
                Find(name).source.Stop();
                currentMusic = "";
            }
        }
    }

    public void StopCurrentMusic()
    {
        if (Find(currentMusic).source.isPlaying)
            Find(currentMusic).source.Stop();
    }
}