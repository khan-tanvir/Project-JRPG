using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    #region Private Fields

    private List<Cutscene> _cutscenes = new List<Cutscene>();

    [SerializeField]
    private float[] _position = { -6.96f, -2.52f };

    [SerializeField]
    private int _questProgress;

    private List<SceneObject> _sceneObjects;

    #endregion Private Fields

    #region Public Properties

    public List<Cutscene> Cutscenes
    {
        get => _cutscenes;
        set => _cutscenes = value;
    }

    public List<InventoryItem> InventoryItems
    {
        get;
        set;
    }

    public string PlayerName
    {
        get;
        set;
    }

    public float[] PlayerPosition
    {
        get => _position;
        set => _position = value;
    }

    public int PlayerQuestProgress
    {
        get => _questProgress;
        set => _questProgress = value;
    }

    public List<SceneObject> SceneObjectsList
    {
        get => _sceneObjects;
        set => _sceneObjects = value;
    }

    #endregion Public Properties
}