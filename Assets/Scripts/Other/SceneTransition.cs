using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    #region Private Fields

    [SerializeField]
    private int _checkPointID;

    [SerializeField]
    private string _scene;

    #endregion Private Fields

    #region Private Methods

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManagerScript.Instance.SceneToGoTo(_scene);

        EventsManager.Instance.CheckPointCall(_checkPointID);
    }

    #endregion Private Methods
}