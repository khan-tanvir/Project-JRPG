using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IDGenerator : MonoBehaviour
{
    #region Private Fields

    [SerializeField]
    private bool _inInventory;

    [SerializeField]
    private bool _isDestroyed;

    [SerializeField]
    private string _objectID;

    [HideInInspector]
    private Vector2 _originalPosition;

    #endregion Private Fields

    #region Public Properties

    public bool InInventory
    {
        get;
        set;
    }

    public Vector2 InitialPosition
    {
        get => _originalPosition;
        set => _originalPosition = value;
    }

    public IDGenerator Instance
    {
        get;
        set;
    }

    public bool IsDestroyed
    {
        get;
        set;
    }

    //private Vector3 _originalRotation;
    public string ObjectID
    {
        get { return _objectID; }
        set { _objectID = value; }
    }

    public float[] Position
    {
        get;
        set;
    }

    #endregion Public Properties

    #region Private Methods

    private void Awake()
    {
        Instance = this;

        string objectPos = transform.position.x.ToString() + "-" + transform.position.y.ToString() + "-" + transform.position.z.ToString();
        string objectRot = transform.rotation.x.ToString() + "-" + transform.rotation.y.ToString() + "-" + transform.rotation.z.ToString();

        ObjectID = "-" + objectPos + "-" + objectRot + "-" + transform.position.magnitude.ToString();

        _originalPosition = transform.position;

        // _originalRotation = transform.rotation;
    }

    #endregion Private Methods

    #region Public Methods

    // called on deletion and save
    public void Save()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);

        Position = new float[2] { pos.x, pos.y };

        if (_originalPosition != pos || IsDestroyed || InInventory)
        {
            SceneManagerScript.Instance.AddToSceneObjectList(this);
        }
    }

    #endregion Public Methods
}