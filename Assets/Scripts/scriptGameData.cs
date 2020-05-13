using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using ReadOnlyAttribute = Unity.Collections.ReadOnlyAttribute;

public class scriptGameData : MonoBehaviour
{
    // Saving and Loading Data
    // Singleton Class

    // Hold reference to this object
    public static scriptGameData gameData;

    // Variables to save
    private string _playerName;

    private float[] _position = new float[2];

    // TODO: Revisit this if we are not going with linear progression
    private int _questProgress;

    // Track if save file exists
    private int[] _existingSaveFiles = { 0, 0, 0 };

    // Track current save file
    private int _currentSaveFile = -1;

    public int CurrentSaveFile
    {
        get { return _currentSaveFile; }
    }

    public string PlayerName
    {
        get { return _playerName; }
        set { _playerName = value; }
    }

    public float[] PlayerPosition
    {
        get { return _position; }
        set { _position = value; }
    }

    public int PlayerQuestProgress
    {
        get { return _questProgress; }
        set { _questProgress = value; }
    }

    public int[] ExistingSaveFiles
    {
        get { return _existingSaveFiles; }
    }

    public float MasterVolume
    {
        get { return PlayerPrefs.GetFloat("Master Volume"); }
    }

    public float MusicVolume
    {
        get { return PlayerPrefs.GetFloat("Music Volume"); }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        if (gameData == null)
        {
            DontDestroyOnLoad(gameObject);
            gameData = this;
        }
        else if (gameData != this)
            Destroy(gameObject);

        //if (SceneManager.GetActiveScene().path == SceneManager.GetSceneByBuildIndex(0).path)
        //    CheckAllFiles();
    }

    private void Start()
    {
        if (PlayerPrefs.GetFloat("Master Volume", 0.8f) < 0.0001f || float.IsNaN(PlayerPrefs.GetFloat("Master Volume")))
            PlayerPrefs.SetFloat("Master Volume", 0.8f);

        if (PlayerPrefs.GetFloat("Music Volume", 0.8f) < 0.0001f || float.IsNaN(PlayerPrefs.GetFloat("Music Volume")))
            PlayerPrefs.SetFloat("Music Volume", 0.8f);
    }

    public void CreateData(int pos)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.persistentDataPath + "/playerInfo" + pos + ".dat");

        // Only storing the name because this is called once after every save file creation
        PlayerData playerData = new PlayerData();
        playerData.PlayerName = PlayerName;

        // Serialise the data
        binaryFormatter.Serialize(fileStream, playerData);

        fileStream.Close();

        Debug.Log("Created save file");
        CheckAllFiles();
    }

    public void SaveData()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        // This stores it within the appdata folder
        FileStream fileStream = File.Create(Application.persistentDataPath + "/playerInfo" + _currentSaveFile + ".dat");

        // Store the data to the PlayerData class
        PlayerData playerData = new PlayerData();
        playerData.PlayerName = PlayerName;
        playerData.PlayerPosition = PlayerPosition;
        playerData.PlayerQuestProgress = PlayerQuestProgress;

        // Serialise the data
        binaryFormatter.Serialize(fileStream, playerData);

        fileStream.Close();

        Debug.Log("Player data has been saved");
    }

    public void LoadData(int pos)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Open(Application.persistentDataPath + "/playerInfo" + pos + ".dat", FileMode.Open);

        // Cast the file data as PlayerData before we initialise it to data
        PlayerData data = (PlayerData)binaryFormatter.Deserialize(fileStream);

        fileStream.Close();

        // Load the variables
        PlayerName = data.PlayerName;

        // If we call LoadData after CreateData then we'll need to deal with the player position var being null
        if (data.PlayerPosition != null)
            PlayerPosition = data.PlayerPosition;

        PlayerQuestProgress = data.PlayerQuestProgress;

        //Track the save file
        _currentSaveFile = pos;

        Debug.Log("Loading Save File " + pos);

        // Stop music before changing scenes
        scriptAudioManager.audioManager.StopCurrentMusic();

        SceneManager.LoadScene(SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/Game.unity"));
    }

    public void ButtonPressed()
    {
        GameObject buttonObject = EventSystem.current.currentSelectedGameObject;
        GameObject fileMenu = GameObject.Find("Canvas").transform.Find("File Menu").gameObject;
        GameObject fileCreationMenu = GameObject.Find("Canvas").transform.Find("File Creation Menu").gameObject;

        switch (buttonObject.name)
        {
            case "File 1 Button":
                if (_existingSaveFiles[0] == 0)
                {
                    fileMenu.SetActive(false);
                    fileCreationMenu.SetActive(true);

                    //Track the save file
                    _currentSaveFile = 0;
                }
                else
                    LoadData(0);
                break;

            case "File 2 Button":
                if (_existingSaveFiles[1] == 0)
                {
                    fileMenu.SetActive(false);
                    fileCreationMenu.SetActive(true);

                    //Track the save file
                    _currentSaveFile = 1;
                }
                else
                    LoadData(1);
                break;

            case "File 3 Button":
                if (_existingSaveFiles[2] == 0)
                {
                    fileMenu.SetActive(false);
                    fileCreationMenu.SetActive(true);

                    //Track the save file
                    _currentSaveFile = 2;
                }
                else
                    LoadData(2);
                break;
            // TODO: Find a better way to do this
            case "Delete File 1 Button":
                if (_existingSaveFiles[0] == 1)
                {
                    DeleteData(0);
                }
                break;
            case "Delete File 2 Button":
                if (_existingSaveFiles[1] == 1)
                {
                    DeleteData(1);
                }
                break;
            case "Delete File 3 Button":
                if (_existingSaveFiles[2] == 1)
                {
                    DeleteData(2);
                }
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
                    _playerName = input;
                    Debug.Log("Player name is " + _playerName);

                    CreateData(_currentSaveFile);
                    break;
                }
        }
    }

    public void CheckAllFiles()
    {
        // Small Optimisation
        Transform fileMenu = GameObject.Find("File Menu").transform;

        for (int i = 0; i < 3; i++)
        {
            TMP_Text temp = fileMenu.Find("File " + (i + 1) + " Text").GetComponent<TMP_Text>();

            if (File.Exists(Application.persistentDataPath + "/playerInfo" + i + ".dat"))
            {
                _existingSaveFiles[i] = 1;
                temp.text = DisplayName(i);
            }
            else
            {
                _existingSaveFiles[i] = 0;
                temp.text = "[BLANK]";
            }   
        }

        _currentSaveFile = -1;
    }

    private string DisplayName(int pos)
    {
        // Get the player name of the file
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Open(Application.persistentDataPath + "/playerInfo" + pos + ".dat", FileMode.Open);

        // Cast the file data as PlayerData before we can do something with it
        PlayerData data = (PlayerData)binaryFormatter.Deserialize(fileStream);

        fileStream.Close();

        // Load the variables
        return data.PlayerName;
    }

    public void ClearData()
    {
        // This function will be called when the player goes back to main menu from the game scene
        PlayerName = "";
        PlayerPosition = new float[2];
        PlayerQuestProgress = 0;
        _currentSaveFile = -1;
    }

    public void  DeleteData(int pos)
    {
        File.Delete(Application.persistentDataPath + "/playerInfo" + pos + ".dat");
        CheckAllFiles();
    }

    public void SaveMasterVolumeValue(float value)
    {
        // Called everytime the user changes the master volume slider
        PlayerPrefs.SetFloat("Master Volume", value);
    }

    public void SaveMusicVolumeValue(float value)
    {
        // Called everytime the user changes the master volume slider
        PlayerPrefs.SetFloat("Music Volume", value);
    }
}

[Serializable]
internal class PlayerData
{
    private string _playerName;
    private float[] _position;
    private int _questProgress;

    public string PlayerName
    {
        get { return _playerName; }
        set { _playerName = value; }
    }

    public float[] PlayerPosition
    {
        get { return _position; }
        set { _position = value; }
    }

    public int PlayerQuestProgress
    {
        get { return _questProgress; }
        set { _questProgress = value; }
    }
}