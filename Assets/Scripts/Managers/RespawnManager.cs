using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    #region Private Fields

    [SerializeField]
    private List<CheckPoint> _checkPoints;

    private int _checkPointToGoTo;

    [SerializeField]
    private Vector2 _currentCheckpoint;

    [SerializeField]
    private Vector2 _defaultPosition;

    #endregion Private Fields

    #region Public Properties

    public static RespawnManager Instance
    {
        get;
        internal set;
    }

    public List<CheckPoint> CheckPoints
    {
        get { return _checkPoints; }
        set { _checkPoints = value; }
    }

    public Vector2 CurrentCheckpoint
    {
        get { return _currentCheckpoint; }
    }

    #endregion Public Properties

    #region Private Methods

    private void Awake()
    {
        CreateInstance();
    }

    private void CreateInstance()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }

    private void LoadCheckPoint()
    {
        if (GameData.Instance.PlayerData != null)
            _currentCheckpoint = new Vector2(GameData.Instance.PlayerData.PlayerPosition[0], GameData.Instance.PlayerData.PlayerPosition[1]);
    }

    private void SceneTransitionCall(int num)
    {
        _checkPointToGoTo = num;
    }

    private void Start()
    {
        GetCheckPoints();
        LoadCheckPoint();

        EventsManager.Instance.OnSceneChange += GetCheckPoints;
        EventsManager.Instance.OnCheckPointCall += SceneTransitionCall;
    }

    #endregion Private Methods

    #region Public Methods

    public void GetCheckPoints()
    {
        List<CheckPoint> points = new List<CheckPoint>();

        foreach (CheckPoint checkPoint in FindObjectsOfType<CheckPoint>())
        {
            points.Add(checkPoint);
            Debug.Log(checkPoint.name);
        }

        CheckPoints = points;

        if (_checkPointToGoTo != 0)
        {
            SetActiveCheckPoint(GetCheckPointWithID(_checkPointToGoTo));
        }
    }

    public CheckPoint GetCheckPointWithID(int id)
    {
        _checkPointToGoTo = 0;
        return CheckPoints.Find(a => a.gameObject.name == id.ToString());
    }

    public void OnLevelWasLoaded(int level)
    {
        EventsManager.Instance.SceneChange();
    }

    public void SceneReload()
    {
        if (_currentCheckpoint == Vector2.zero)
        {
            return;
        }

        SetActiveCheckPoint(CheckPoints.Find(a => a.Position == _currentCheckpoint));
    }

    public void SetActiveCheckPoint(CheckPoint checkPoint)
    {
        GetCheckPoints();

        if (checkPoint == null || !CheckPoints.Contains(checkPoint))
        {
            return;
        }

        foreach (CheckPoint point in CheckPoints.GetRange(0, CheckPoints.IndexOf(checkPoint)))
        {
            point.CheckPointTriggered = true;
        }

        _currentCheckpoint = checkPoint.Position;
    }

    #endregion Public Methods
}