using UnityEngine;

[System.Serializable]
public class CheckPoint : MonoBehaviour
{
    #region Private Fields

    private Vector2 _position;

    [SerializeField]
    private bool _triggered;

    #endregion Private Fields

    #region Public Properties

    public bool CheckPointTriggered
    {
        get => _triggered;
        set => _triggered = value;
    }

    public Vector2 Position
    {
        get => _position;
        set => _position = value;
    }

    #endregion Public Properties

    #region Private Methods

    private void Awake()
    {
        CheckPointTriggered = false;
        _position = transform.position;
    }

    private void Death()
    {
        Debug.Log("Death func called");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!CheckPointTriggered)
        {
            if (other.CompareTag("Player"))
            {
                CheckPointTriggered = true;
                RespawnManager.Instance.SetActiveCheckPoint(this);
            }
        }
    }

    #endregion Private Methods
}