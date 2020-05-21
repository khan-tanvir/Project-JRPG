using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationEntered : MonoBehaviour
{
    // WIP Class
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // DEBUG
        EventsManager.Instance.LocationEntered(other.name);
    }
}
