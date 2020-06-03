using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenu : MonoBehaviour
{
    #region Public Methods

    public void GiveUp()
    {
        FindObjectOfType<PauseMenu>().Quit();
    }

    public void Retry()
    {
        SceneManagerScript.Instance.ReloadScene();
    }

    #endregion Public Methods
}