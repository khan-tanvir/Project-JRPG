using UnityEngine;

public class FileMenu : MonoBehaviour
{
    #region Private Methods

    // Start is called before the first frame update
    private void OnEnable()
    {
        AudioManager.Instance.PlayMusic("FileSelection");
        AudioManager.Instance.EnableMusicLoop();
    }

    #endregion Private Methods
}