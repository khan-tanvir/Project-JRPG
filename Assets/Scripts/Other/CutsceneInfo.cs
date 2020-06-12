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

    [System.NonSerialized]
    public CutsceneInfo CutsceneMB;

    public bool HasPlayed;

    public bool InfiniteUses;

    #endregion Public Fields
}

public class CutsceneInfo : MonoBehaviour
{
    #region Private Fields

    private bool _isPlaying;

    #endregion Private Fields

    #region Public Fields

    public Cutscene Cutscene;

    #endregion Public Fields

    #region Private Methods

    private void Awake()
    {
        Cutscene.CSname = GetComponent<PlayableDirector>().playableAsset.name;
        Cutscene.CutsceneMB = this;
    }

    private void IsFinished(PlayableDirector director)
    {
        EventsManager.Instance.CutsceneFinish(Cutscene.CSname);

        GetComponent<PlayableDirector>().stopped -= IsFinished;
    }

    public void PlayCS()
    {
        if (GetComponent<PlayableDirector>() != null && GetComponent<PlayableDirector>().playableAsset != null)
        {
            GetComponent<PlayableDirector>().Play();

            GetComponent<PlayableDirector>().stopped += IsFinished;
        }
    }

    #endregion Private Methods
}