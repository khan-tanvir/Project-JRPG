using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        scriptAudioManager.audioManager.PlayMusic("Theme");
        scriptAudioManager.audioManager.EnableMusicLoop();
    }
}
