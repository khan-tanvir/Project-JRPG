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
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    // If game is paused then unpaused it once esc key is pressed
        //    if (_isPaused)
        //    {
        //        Resume();
        //    }
        //    else
        //    {
        //        Pause();
        //    }
        //}
    }

    public void Pause()
    {
        _pauseMenu.SetActive(true);
        // Freeze the game
        Time.timeScale = 0.0f;
        _isPaused = true;
    }

    public void Resume()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        _isPaused = false;
    }

    public void Toggle()
    {
        if (_isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void SavePlayerProgress()
    {
        // Before saving make sure you have loaded a file first

        // Store the position
        scriptGameData.GameDataManager.PlayerPosition[0] = FindObjectOfType<scriptPlayer>().rigidBody2D.position.x;
        scriptGameData.GameDataManager.PlayerPosition[1] = FindObjectOfType<scriptPlayer>().rigidBody2D.position.y;

        Debug.Log("X: " + scriptGameData.GameDataManager.PlayerPosition[0] + " Y: " + scriptGameData.GameDataManager.PlayerPosition[1]);

        scriptInventory.Inventory.SaveInventory();

        scriptGameData.GameDataManager.SaveData();
    }
}
