using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Playables;

[System.Serializable]
public class Cutscene
{
    #region Public Fields

    [HideInInspector]
    public string CSname;

    public CutsceneInfo CutsceneMB;

    public bool HasPlayed;

    #endregion Public Fields
}

public class CutsceneInfo : MonoBehaviour
{
    #region Public Fields

    public Cutscene Cutscene;

    #endregion Public Fields

    #region Private Methods

    private void Start()
    {
        Cutscene.CSname = GetComponent<PlayableDirector>().playableAsset.name;
        Cutscene.CutsceneMB = this;

        CutSceneManager.Instance.PlayCutScene(Cutscene.CSname);
    }

    public void PlayCS()
    {
        GetComponent<PlayableDirector>().Play();
    }

    #endregion Private Methods
}