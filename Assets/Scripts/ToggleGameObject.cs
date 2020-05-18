using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGameObject : MonoBehaviour
{
   public bool ToggleObject()
    {
        Debug.Log("Function called");
        gameObject.SetActive(!gameObject.activeInHierarchy);

        return gameObject.activeInHierarchy;
    }
}
