using System;
using UnityEngine;
using UnityEngine.Audio;

public class scriptAudioManager : MonoBehaviour
{
    // TODO: Fade-In and Fade-Out
    // TODO: Sound Effects
    
    public AudioMixer audioMixer;

    // Store which music is playing
    public string currentMusic;

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master Volume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music Volume", volume);
    }

    // Hold reference to this object
    public scriptAudioManager audioManager;

    // Array that stores all sounds
    public scriptSound[] sounds;

    private void Awake()
    {
        if (audioManager == null)
        {
            DontDestroyOnLoad(gameObject);
            audioManager = this;
        }
        else if (audioManager != null)
            Destroy(audioManager);

        foreach (scriptSound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.outputAudioMixerGroup = audioMixer.FindMatchingGroups(sound.audioType)[0];
            sound.source.clip = sound.clip;
            sound.source.name = sound.name;
            sound.source.volume = sound.volume;
            sound.source.mute = sound.mute;
        }
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
            return true;
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