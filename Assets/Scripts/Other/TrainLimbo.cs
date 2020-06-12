using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TrainLimbo : MonoBehaviour
{
    #region Private Methods

    private void Setup(string name)
    {
        if (name != "TrainCutscene")
        {
            return;
        }

        EventsManager.Instance.OnCutsceneFinish -= Setup;

        AudioManager.Instance.PlayMusic("GameScene");
    }

    private void Start()
    {
        CutSceneManager.Instance.GetCutscenes();
        if (!CutSceneManager.Instance.GetCutsceneByName("TrainCutscene").HasPlayed)
        {
            CutSceneManager.Instance.PlayCutScene("TrainCutscene");
        }
        else
        {
            CutSceneManager.Instance.GetCutsceneByName("TrainCutscene").CutsceneMB.PlayCS();
            CutSceneManager.Instance.GetCutsceneByName("TrainCutscene").CutsceneMB.GetComponent<PlayableDirector>().time = 100.0;
        }

        EventsManager.Instance.OnCutsceneFinish += Setup;
    }

    #endregion Private Methods
}