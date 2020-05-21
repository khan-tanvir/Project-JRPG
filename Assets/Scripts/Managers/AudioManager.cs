﻿using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    // TODO: Fade-In and Fade-Out
    // TODO: Sound Effects

    [SerializeField]
    private AudioMixer _audioMixer;

    [SerializeField]
    private Slider _masterSlider;

    [SerializeField]
    private Slider _musicSlider;

    // Array that stores all sounds
    [SerializeField]
    private Sound[] _sounds;

    // Hold reference to this object
    public static AudioManager Instance
    {
        get;
        internal set;
    }

    // Store which music is playing
    public string CurrentMusic
    {
        get;
        internal set;
    }

    public Sound[] Sounds
    {
        get { return _sounds; }
    }

    public Sound[] sounds;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != null)
            Destroy(gameObject);

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.outputAudioMixerGroup = _audioMixer.FindMatchingGroups(Enum.GetName(typeof(SoundType), sound.Type))[0];
            sound.source.clip = sound.Clip;
            sound.source.name = sound.Name;
            sound.source.volume = sound.volume;
        }

        // Countering a bug that changes the name to the 2nd element of sounds
        gameObject.name = "Audio Manager";
    }

    public void Start()
    {
        if (_masterSlider != null && _musicSlider != null)
        {
            _masterSlider.value = GameData.Instance.MasterVolume;
            _musicSlider.value = GameData.Instance.MusicVolume;
        }
    }

    public void SetMasterVolume(float volume)
    {
        // Using log10 for a better slider
        _audioMixer.SetFloat("Master Volume", Mathf.Log10(volume) * 20.0f);
    }

    public void SetMusicVolume(float volume)
    {
        _audioMixer.SetFloat("Music Volume", Mathf.Log10(volume) * 20.0f);
    }

    public Sound Find(string name)
    {
        Sound soundToFind = Array.Find(sounds, clip => clip.Name == name);
        return soundToFind;
    }

    public void PlayMusic(string name)
    {
        if (Find(name) != null)
        {
            if (CurrentMusic == name)
            {
                return;
            }
            else
            {
                if (!(string.IsNullOrEmpty(CurrentMusic)))
                {
                    Find(CurrentMusic).source.Stop();
                }

                Find(name).source.Play();
                CurrentMusic = name;
            }
        }
    }

    public bool IsPlaying(string name)
    {
        if (Find(name) != null)
        {
            if (Find(name).source.isPlaying)
            {
                return true;
            }
        }

        return false;
    }

    public void EnableMusicLoop()
    {
        Find(CurrentMusic).source.loop = true;
    }

    public void DisableMusicLoop()
    {
        Find(CurrentMusic).source.loop = false;
    }

    public void StopMusic(string name)
    {
        if (IsPlaying(name))
        {
            Find(name).source.Stop();
            if (name == CurrentMusic)
                CurrentMusic = "";
        }
    }

    public void StopCurrentMusic()
    {
        if (IsPlaying(CurrentMusic))
            Find(CurrentMusic).source.Stop();
    }
}