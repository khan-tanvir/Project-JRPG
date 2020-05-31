using System.Security.Cryptography;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    #region Public Fields

    public GameObject _pauseMenu;

    #endregion Public Fields

    #region Public Properties

    public static bool GameIsPaused
    {
        get;
        internal set;
    }

    #endregion Public Properties

    #region Public Methods

    public void Pause()
    {
        _pauseMenu.SetActive(true);

        // Freeze the game
        Time.timeScale = 0.0f;
        GameIsPaused = true;
    }

    public void Quit()
    {
        Resume();
        Destroy(QuestManager.Instance.gameObject);
        Destroy(EventsManager.Instance.gameObject);
        Destroy(RespawnManager.Instance.gameObject);

        SceneManagerScript.Instance.SceneToGoTo("Menu");
    }

    public void Resume()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
    }

    public void SavePlayerProgress()
    {
        if (GameData.Instance.PlayerData == null)
        {
            Debug.LogError("You need to load a save file first");
            return;
        }
        
        // Before saving make sure you have loaded a file first

        // Store the position
        GameData.Instance.PlayerData.PlayerPosition[0] = RespawnManager.Instance.CurrentCheckpoint.x;
        GameData.Instance.PlayerData.PlayerPosition[1] = RespawnManager.Instance.CurrentCheckpoint.y;

        Inventory.Instance.SaveInventory();

        foreach (IDGenerator idgen in Resources.FindObjectsOfTypeAll<IDGenerator>())
        {
            if (idgen == null || idgen.ObjectID == "")
            {
                continue;
            }

            idgen.Save();
        }

        GameData.Instance.SaveData();
    }

    public void Toggle()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    #endregion Public Methods
}