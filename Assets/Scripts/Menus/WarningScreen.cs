using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningScreen : MonoBehaviour
{
    #region Private Fields

    private bool _startTimer;

    #endregion Private Fields

    #region Public Properties

    public Animator Animator
    {
        get => GetComponent<Animator>();
    }

    public string Scene
    {
        get;
        set;
    }

    #endregion Public Properties

    #region Public Methods

    public void OnWarningFinished()
    {
        if (!string.IsNullOrEmpty(Scene))
        {
            SceneManagerScript.Instance.SceneToGoTo(Scene);
        }
        else
        {
            if (GameData.Instance.PlayerData.Cutscenes.Count != 0)
            {
                SceneManagerScript.Instance.SceneToGoTo("Train");
            }
            else
            {
                SceneManagerScript.Instance.SceneToGoTo("Station Entrance");
            }
        }
    }

    #endregion Public Methods
}