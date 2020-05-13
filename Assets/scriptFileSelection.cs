using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptFileSelection : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        scriptAudioManager.audioManager.PlayMusic("FileSelection");
        scriptAudioManager.audioManager.EnableMusicLoop();
    }
}
