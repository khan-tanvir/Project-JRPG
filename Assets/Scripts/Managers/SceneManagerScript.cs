using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    #region Public Properties

    public static SceneManagerScript Instance
    {
        get;
        internal set;
    }

    public string CurrentScene
    {
        get;
        internal set;
    }

    public List<SceneObject> SceneObjects
    {
        get;
        internal set;
    }

    #endregion Public Properties

    #region Private Methods

    private void Awake()
    {
        CreateInstance();

        SceneObjects = new List<SceneObject>();
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

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;

        if (EventsManager.Instance != null)
        {
            EventsManager.Instance.OnSceneChange -= ExamineScene;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode sceneMode)
    {
        EventsManager.Instance?.SceneChange();
        ExamineScene();
    }

    #endregion Private Methods

    #region Public Methods

    public void AddToSceneObjectList(IDGenerator idgen)
    {
        if (idgen.ObjectID != null)
        {
            int index = SceneObjects.FindIndex(a => a.ID == idgen.ObjectID);

            if (index != -1)
            {
                // matching element so update other values
                SceneObjects[index].IsDestroyed = idgen.IsDestroyed;
                SceneObjects[index].InInventory = idgen.InInventory;
                SceneObjects[index].Position = idgen.Position;
            }
            else
            {
                // no matching element, this is a new element
                SceneObject sceneObject = new SceneObject();

                sceneObject.ID = idgen.ObjectID;
                sceneObject.IsDestroyed = idgen.IsDestroyed;
                sceneObject.InInventory = idgen.InInventory;
                sceneObject.Position = idgen.Position;

                SceneObjects.Add(sceneObject);
            }
        }
    }

    public void ExamineScene()
    {
        if (GameData.Instance.PlayerData == null)
        {
            return;
        }

        foreach (IDGenerator idgen in Resources.FindObjectsOfTypeAll<IDGenerator>())
        {
            if (idgen == null || string.IsNullOrEmpty(idgen.ObjectID))
            {
                continue;
            }

            int index = SceneObjects.FindIndex(a => a.ID == idgen.ObjectID);

            // if a match is found
            if (index != -1)
            {
                if (SceneObjects[index].IsDestroyed)
                {
                    // check if its marked as destroyed
                    AddToSceneObjectList(idgen);
                    Destroy(idgen.gameObject);
                }
                else if (SceneObjects[index].InInventory)
                {
                    Destroy(idgen.gameObject);
                    //// Check if its not already in the list
                    //if (SceneObjects.FindIndex(a => a.ID == idgen.ObjectID) != -1)
                    //{
                    //    Destroy(idgen.gameObject);
                    //}

                    //AddToSceneObjectList(idgen);
                }
                else
                {
                    // If inventory and destroyed are false, that means its position has been changed
                    idgen.gameObject.transform.position = new Vector3(SceneObjects[index].Position[0], SceneObjects[index].Position[1], -1.0f);
                }
            }
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        RespawnManager.Instance.SceneReload();
        Time.timeScale = 1.0f;
    }

    public void SaveObjects()
    {
        foreach (IDGenerator idgen in Resources.FindObjectsOfTypeAll<IDGenerator>())
        {
            if (idgen == null || string.IsNullOrEmpty(idgen.ObjectID))
            {
                continue;
            }

            idgen.Save();
        }
    }

    public void SceneToGoTo(string sceneName)
    {
        SaveObjects();

        SceneManager.LoadScene(SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/" + sceneName + ".unity"));

        switch (sceneName)
        {
            case "Menu":
                AudioManager.Instance.PlayMusic("MainMenu");
                break;

            case "Game":
                AudioManager.Instance.PlayMusic("GameScene");
                break;

            default:
                break;
        }

        CurrentScene = sceneName;
    }

    #endregion Public Methods
}

[System.Serializable]
public class SceneObject
{
    #region Private Fields

    [SerializeField]
    private string _id;

    [SerializeField]
    private bool _inInventory;

    [SerializeField]
    private bool _isDestroyed;

    [SerializeField]
    private float[] _position = new float[2];

    [SerializeField]
    private float[] _rotation = new float[2];

    #endregion Private Fields

    #region Private Properties

    private float[] Rotation
    {
        get { return _rotation; }
        set { _rotation = value; }
    }

    #endregion Private Properties

    #region Public Properties

    public string ID
    {
        get { return _id; }
        set { _id = value; }
    }

    public bool InInventory
    {
        get { return _inInventory; }
        set { _inInventory = value; }
    }

    public bool IsDestroyed
    {
        get { return _isDestroyed; }
        set { _isDestroyed = value; }
    }

    public float[] Position
    {
        get { return _position; }
        set { _position = value; }
    }

    #endregion Public Properties
}