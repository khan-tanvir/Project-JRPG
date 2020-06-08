using UnityEngine;

public class Settings : MonoBehaviour
{
    void Update()
    {
        Graphics();
    }

    public void Graphics()
    {
        setResolution();
    }

   void setResolution()
    {
        // Getting the name of what is pressed
        string index = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        switch (index)
        {
            case "0":
                Screen.SetResolution(1920, 1080, true, 60);
                break;

            case "1":
                Screen.SetResolution(1680, 1050, true, 60);
                break;

            case "2":
                Screen.SetResolution(960, 720, true, 60);
                break;
        }

    }
}