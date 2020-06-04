using UnityEngine;

public class Settings : MonoBehaviour
{
    void Awake()
    {
        Resolution[] resolutionsArray = Screen.resolutions;

        // Print the resolutions
        foreach (var res in resolutionsArray)
        {
            Debug.Log(res.width + "x" + res.height + " : " + res.refreshRate);
        }
    }
}