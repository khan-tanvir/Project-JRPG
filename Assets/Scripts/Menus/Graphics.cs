using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Graphics : MonoBehaviour
{
    #region Private Fields

    private Resolution[] resolutionsArray;

    #endregion Private Fields

    #region Public Fields

    public TMP_Dropdown _Dropdown;

    #endregion Public Fields

    #region Private Methods

    private void Start()
    {
        resolutionsArray = Screen.resolutions;
        _Dropdown.ClearOptions();

        List<string> Options = new List<string>();
        int CurrentResolutionIndex = 0;

        for (int i = 0; i < resolutionsArray.Length; i++)
        {
            string displayOption = resolutionsArray[i].width + "X" + resolutionsArray[i].height;
            Options.Add(displayOption);
            if (resolutionsArray[i].width == Screen.currentResolution.width && resolutionsArray[i].height == Screen.currentResolution.height)
            {
                CurrentResolutionIndex = i;
            }
        }

        _Dropdown.AddOptions(Options);
        _Dropdown.value = CurrentResolutionIndex;
        _Dropdown.RefreshShownValue();
    }

    #endregion Private Methods

    #region Public Methods

    public void SetResolution(int ResolutionIndex)
    {
        Resolution resolution = resolutionsArray[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    #endregion Public Methods
}