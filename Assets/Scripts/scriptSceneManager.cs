using UnityEngine.SceneManagement;
using UnityEngine;

public class scriptSceneManager : MonoBehaviour
{
    public void SceneToGoTo(string sceneName)
    {
        SceneManager.LoadScene(SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/" + sceneName + ".unity"));
    }
}
