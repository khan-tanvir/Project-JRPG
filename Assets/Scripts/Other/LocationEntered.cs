using UnityEngine;

public class LocationEntered : MonoBehaviour
{
    // WIP Class

    #region Private Methods

    private void OnTriggerEnter2D(Collider2D other)
    {
        // DEBUG
        EventsManager.Instance.LocationEntered(gameObject.name);
        EventsManager.Instance.InteractionWithItem(gameObject.name);
    }

    #endregion Private Methods
}