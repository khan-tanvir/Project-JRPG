using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    #region Public Properties

    public static SceneManagerScript Instance
    {
        get;
        internal set;
    }

    #endregion Public Properties

    #region Private Methods

    private void Start()
    {
        Instance = this;
    }

    #endregion Private Methods

    #region Public Methods

    public void SceneToGoTo(string sceneName)
    {
        SceneManager.LoadScene(SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/" + sceneName + ".unity"));

        switch (sceneName)
        {
            case "Menu":
                AudioManager.Instance.PlayMusic("MainMenu");
                break;

            case "Game":
                AudioManager.Instance.PlayMusic("GameScene");
                break;

            default:
                break;
        }
    }

    #endregion Public Methods
}