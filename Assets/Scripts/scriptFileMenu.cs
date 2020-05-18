using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptFileMenu : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        scriptAudioManager.audioManager.PlayMusic("FileSelection");
        scriptAudioManager.audioManager.EnableMusicLoop();
    }
}
