using UnityEngine;

public class Journal : MonoBehaviour
{
    #region Private Methods

    // Start is called before the first frame update
    private void OnEnable()
    {
        QuestManager.Instance.ClearCurrentQuest();
    }

    #endregion Private Methods
}