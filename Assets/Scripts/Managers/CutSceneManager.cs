using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutSceneManager : MonoBehaviour
{
    #region Private Methods

    private void Awake()
    {
        CreateInstance();

        if (Cutscenes == null)
        {
            Cutscenes = new List<Cutscene>();
        }
    }

    private void CreateInstance()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        EventsManager.Instance.OnSceneChange -= GetCutscenes;
    }

    private void OnEnable()
    {
        EventsManager.Instance.OnSceneChange += GetCutscenes;
    }

    #endregion Private Methods

    #region Public Properties

    public static CutSceneManager Instance
    {
        get;
        internal set;
    }

    public List<Cutscene> Cutscenes
    {
        get;
        set;
    }

    #endregion Public Properties

    #region Public Methods

    private void CompareToList(List<Cutscene> cutscenes)
    {
        foreach (Cutscene cutscene in cutscenes)
        {
            Debug.Log(cutscene.CSname);

            int index = Cutscenes.FindIndex(a => a.CSname == cutscene.CSname);

            if (index != -1)
            {
                Cutscenes[index].HasPlayed = cutscene.HasPlayed;
            }
        }
    }

    // This function will be called on scene load
    public void GetCutscenes()
    {
        foreach (CutsceneInfo cs in FindObjectsOfType<CutsceneInfo>())
        {
            if (Cutscenes.Find(a => a == cs.Cutscene) == null)
            {
                Cutscenes.Add(cs.Cutscene);
            }
        }

        if (GameData.Instance.PlayerData != null)
        {
            CompareToList(GameData.Instance.PlayerData.Cutscenes);
        }
    }

    public void PlayCutScene(string name)
    {
        int index = Cutscenes.FindIndex(a => a.CSname == name);

        if (index != -1)
        {
            if (!Cutscenes[index].HasPlayed)
            {
                Cutscenes[index].CutsceneMB.PlayCS();
                Cutscenes[index].HasPlayed = true;
            }
        }
    }

    #endregion Public Methods
}