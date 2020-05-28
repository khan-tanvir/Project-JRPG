using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Graphics : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Dropdown tMP_Dropdown;
    Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;
        tMP_Dropdown.ClearOptions();

        List<string> Resolution = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string Resolutions = resolutions[i].width + " " + "X" + " " + resolutions[i].height;
            Resolution.Add(Resolutions);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        tMP_Dropdown.AddOptions(Resolution);
        tMP_Dropdown.value = currentResolutionIndex;
        tMP_Dropdown.RefreshShownValue();

    }

    public void setResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void setGraphics(int graphicIndex)
    {
        QualitySettings.SetQualityLevel(graphicIndex);
    }
}
