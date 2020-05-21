using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MethodCaller : MonoBehaviour
{
    // This class is used to call functions via inspector

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