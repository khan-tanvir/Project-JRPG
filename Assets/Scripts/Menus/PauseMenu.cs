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
        // Before saving make sure you have loaded a file first

        // Store the position
        GameData.Instance.PlayerData.PlayerPosition[0] = FindObjectOfType<Player>().RigidBody.position.x;
        GameData.Instance.PlayerData.PlayerPosition[1] = FindObjectOfType<Player>().RigidBody.position.y;

        Debug.Log("X: " + GameData.Instance.PlayerData.PlayerPosition[0] + " Y: " + GameData.Instance.PlayerData.PlayerPosition[1]);

        Inventory.Instance.SaveInventory();

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