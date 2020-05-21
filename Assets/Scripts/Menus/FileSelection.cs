using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FileSelection : MonoBehaviour
{
    [SerializeField]
    private GameObject fileMenu;

    [SerializeField]
    private GameObject fileCreationMenu;

    private void OnEnable()
    {
        GameData.Instance.CheckAllFiles();
    }

    public void ButtonPressed()
    {
        GameObject buttonObject = EventSystem.current.currentSelectedGameObject;

        GameData.Instance.CheckAllFiles();
        int[] arr = GameData.Instance.ExistingSaveFiles;

        switch (buttonObject.name)
        {
            case "File 1 Button":
                if (arr[0] == 0)
                {
                    fileCreationMenu.SetActive(true);
                    fileMenu.SetActive(false);

                    GameData.Instance.CurrentSaveFile = 0;
                }
                else
                {
                    GameData.Instance.LoadData(0);
                }
                break;

            case "File 2 Button":
                if (arr[1] == 0)
                {
                    fileCreationMenu.SetActive(true);
                    fileMenu.SetActive(false);

                    GameData.Instance.CurrentSaveFile = 1;
                }
                else
                {
                    GameData.Instance.LoadData(1);
                }
                break;

            case "File 3 Button":
                if (arr[2] == 0)
                {
                    fileCreationMenu.SetActive(true);
                    fileMenu.SetActive(false);

                    GameData.Instance.CurrentSaveFile = 2;
                }
                else
                {
                    GameData.Instance.LoadData(2);
                }
                break;

            case "Delete File 1 Button":
                if (arr[0] == 1)
                    GameData.Instance.DeleteData(0);
                break;

            case "Delete File 2 Button":
                if (arr[1] == 1)
                    GameData.Instance.DeleteData(1);
                break;

            case "Delete File 3 Button":
                if (arr[2] == 1)
                    GameData.Instance.DeleteData(2);
                break;

            case "Confirm Button":
                // Get text component of File Creation Menu's InputField child
                string input = fileCreationMenu.GetComponentInChildren<TMP_InputField>().text;
                // Input Validation
                if (string.IsNullOrEmpty(input))
                {
                    // Do something
                    Debug.Log("Empty name input");
                    break;
                }
                else if (!(input.All(Char.IsLetter)))
                {
                    Debug.Log("Invalid input");
                    break;
                }
                else if (input.Length > 11 || input.Length < 3)
                {
                    Debug.Log("Input is either too long or too short");
                    break;
                }
                else
                {
                    fileMenu.SetActive(true);
                    fileCreationMenu.SetActive(false);

                    GameData.Instance.CreateData(input);
                }
                break;
        }
    }
}
