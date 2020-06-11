using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    #region Private Fields

    private bool _hasPlayerBeenPrompted;

    #endregion Private Fields

    #region Public Fields

    [SerializeField]
    private GameObject _pauseMenu;

    [SerializeField]
    private GameObject _savePrompt;

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

    public void PlayerHasBeenPrompted()
    {
        _hasPlayerBeenPrompted = true;
    }

    public void Quit()
    {
        if (!_hasPlayerBeenPrompted)
        {
            _savePrompt.SetActive(true);
            return;
        }

        Resume();
        Destroy(QuestManager.Instance.gameObject);
        Destroy(EventsManager.Instance.gameObject);
        Destroy(RespawnManager.Instance.gameObject);
        Destroy(DialogueManager.Instance.gameObject);

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

        // Store the position
        GameData.Instance.PlayerData.PlayerPosition[0] = RespawnManager.Instance.CurrentCheckpoint.x;
        GameData.Instance.PlayerData.PlayerPosition[1] = RespawnManager.Instance.CurrentCheckpoint.y;

        Inventory.Instance.SaveInventory();

        SceneManagerScript.Instance.SaveObjects();

        GameData.Instance.PlayerData.SceneObjectsList = SceneManagerScript.Instance.SceneObjects;

        GameData.Instance.SaveData();

        _hasPlayerBeenPrompted = true;
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