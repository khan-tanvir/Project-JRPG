using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainLimbo : MonoBehaviour
{
    #region Private Methods

    private void OnEnable()
    {
        AudioManager.Instance.PlayMusic("GameScene");
    }

    #endregion Private Methods
}