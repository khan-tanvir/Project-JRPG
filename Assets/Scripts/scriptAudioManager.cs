using UnityEngine.Audio;
using System;
using UnityEngine;

public class scriptAudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetMasterVolume (float volume)
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

    public void Play(string name)
    {
        Find(name).source.Play();
    }

    public bool IsPlaying(string name)
    {
        if (Find(name).source.isPlaying)
            return true;
        else
            return false;
    }

    public void ToggleLoop(string name)
    {
        if (Find(name).source != null)
        {
            Find(name).source.loop = !Find(name).source.loop;
        }
    }

    public void Stop(string name)
    {
        if (Find(name).source != null)
        { 
            if (Find(name).source.isPlaying)
                Find(name).source.Stop();
        }
    }
}
