using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    #region Public Properties

    public CheckPoint CheckPoint
    {
        get { return _checkPoint; }
    }

    #endregion Public Properties

    #region Private Fields

    [SerializeField]
    private CheckPoint _checkPoint;

    private Vector2 _initialPosition;

    #endregion Private Fields

    #region Private Methods

    // Start is called before the first frame update
    private void Awake()
    {
        _initialPosition = transform.position;

        //EventsManager.Instance.OnRespawn += Respawn;
    }

    #endregion Private Methods

    #region Public Methods

    public void Respawn()
    {
        transform.position = _initialPosition;

        // Character respawned
    }

    #endregion Public Methods
}