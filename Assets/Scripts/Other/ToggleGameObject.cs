using UnityEngine;

public class ToggleGameObject : MonoBehaviour
{
    #region Public Properties

    public bool SetActive
    {
        get { return gameObject.activeInHierarchy; }
        set { gameObject.SetActive(value); }
    }

    #endregion Public Properties
}