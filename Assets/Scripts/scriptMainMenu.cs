using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class scriptMainMenu : MonoBehaviour
{
    public void Awake()
    {
        FindObjectOfType<scriptAudioManager>().Play("Theme");
    }

    public void LoadGameLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Debug.Log("You quit the game.");
        Application.Quit();
    }
}
