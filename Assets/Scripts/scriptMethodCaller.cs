using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class scriptMethodCaller : MonoBehaviour
{
    // This class is used to call functions

    public UnityEvent Functions;

    public void OnEnable()
    {
        if (Functions == null)
            Functions = new UnityEvent();

        FunctionsToCall();
    }

    void FunctionsToCall()
    {
        Functions.Invoke();
    }
}