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
        _searchObjective.OnTriggerEnter2D(other);
        Destroy(this);
    }
}
