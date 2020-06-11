using TMPro;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    #region Private Fields

    private Resolution[] _screenResolutions;

    private int currentRes;

    private List<string> values;

    #endregion Private Fields

    #region Private Methods

    private void LoadResolutions()
    {
        TMP_Dropdown dropdown = GetComponentInChildren<TMP_Dropdown>();

        dropdown.ClearOptions();

        values = new System.Collections.Generic.List<string>();

        for (int i = 0; i < _screenResolutions.Length; i++)
        {
            if (_screenResolutions[i].refreshRate != 60)
            {
                continue;
            }

            string option = _screenResolutions[i].width + " * " + _screenResolutions[i].height;

            values.Add(option);

            if (_screenResolutions[i].width == Screen.currentResolution.width && _screenResolutions[i].height == Screen.currentResolution.height)
            {
                currentRes = i;
            }
        }

        dropdown.AddOptions(values);
        dropdown.value = currentRes;
        dropdown.RefreshShownValue();
    }

    private void OnEnable()
    {
        _screenResolutions = Screen.resolutions;

        LoadResolutions();
    }

    public void SetRes(int value)
    {
        if (values.Count != 0)
        {
            Resolution res = _screenResolutions[value];
            Debug.Log(res);
            Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        }
    }

    #endregion Private Methods
}