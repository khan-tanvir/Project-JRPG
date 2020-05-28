using UnityEngine;
using UnityEngine.Events;

public class MethodCaller : MonoBehaviour
{
    // This class is used to call functions via inspector

    #region Public Fields

    public UnityEvent Functions;

    #endregion Public Fields

    #region Private Methods

    private void FunctionsToCall()
    {
        Functions.Invoke();
    }

    #endregion Private Methods

    #region Public Methods

    public void OnEnable()
    {
        if (Functions == null)
            Functions = new UnityEvent();

        FunctionsToCall();
    }

    #endregion Public Methods
}