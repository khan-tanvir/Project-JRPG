using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileMenu : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        AudioManager.Instance.PlayMusic("FileSelection");
        AudioManager.Instance.EnableMusicLoop();
    }
}
