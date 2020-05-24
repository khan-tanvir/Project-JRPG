using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGameObject : MonoBehaviour
{
   public bool SetActive
    {
        get { return gameObject.activeInHierarchy; }
        set { gameObject.SetActive(value); }
    }
}
