using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptOnTriggerEnter : MonoBehaviour
{
    private SearchObjective _searchObjective;

    public SearchObjective SearchObjective
    {
        get { return _searchObjective; }
        set { _searchObjective = value; }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // DEBUG

        scriptGameEvents._gameEvents.LocationEntered("Cave");
    }
}
