using UnityEngine;

public class Menu : MonoBehaviour
{
    #region Private Methods

    // Start is called before the first frame update
    private void OnEnable()
    {
        AudioManager.Instance.PlayMusic("MainMenu");
        AudioManager.Instance.EnableMusicLoop();
    }

    #endregion Private Methods

    #region Public Methods

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Application quit");
    }

    #endregion Public Methods
}