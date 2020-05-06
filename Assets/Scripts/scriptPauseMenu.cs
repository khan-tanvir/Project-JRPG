using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptPauseMenu : MonoBehaviour
{
    private static bool _isPaused = false;

    public GameObject _pauseMenu;

    public bool GameIsPaused
    {
        get { return _isPaused; }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // If game is paused then unpaused it once esc key is pressed
            if (_isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Resume()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        _isPaused = false;
    }

    void Pause()
    {
        _pauseMenu.SetActive(true);
        // Freeze the game
        Time.timeScale = 0.0f;
        _isPaused = true;
    }

    void SavePlayerProgress()
    {
        FindObjectOfType<scriptGameData>().SaveData();
    }
}
