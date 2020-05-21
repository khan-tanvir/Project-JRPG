using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript Instance
    {
        get;
        internal set;
    }

    private void Start()
    {
        Instance = this;
    }

    public void SceneToGoTo(string sceneName)
    {
        SceneManager.LoadScene(SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/" + sceneName + ".unity"));

        switch(sceneName)
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
}
