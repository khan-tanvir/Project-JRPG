using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        AudioManager.Instance.PlayMusic("MainMenu");
        AudioManager.Instance.EnableMusicLoop();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Application quit");
    }
}
