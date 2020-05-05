using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class scriptGameData : MonoBehaviour
{
    // Saving and Loading Data
    // Singleton Class

    // Hold reference to this object
    public static scriptGameData gameData;

    // Variables to save
    private string _playerName;

    private float[] _position;

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

    // Start is called before the first frame update
    private void Awake()
    {
        if (gameData == null)
        {
            DontDestroyOnLoad(gameObject);
            gameData = this;
        }
        else if (gameData != this)
            Destroy(gameData);

        CheckAllFiles();
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

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ButtonPressed()
    {
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "File1Button":
                if (_existingSaveFiles[0] == 0)
                {
                    // FileMenu GameObject
                    EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);

                    // FileCreationMenu GameObject
                    EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.transform.parent.Find("FileCreationMenu").gameObject.SetActive(true);

                    //Track the save file
                    _currentSaveFile = 0;
                }
                else
                    LoadData(0);
                break;

            case "File2Button":
                if (_existingSaveFiles[1] == 0)
                {
                    // FileMenu GameObject
                    EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);

                    // FileCreationMenu GameObject
                    EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.transform.parent.Find("FileCreationMenu").gameObject.SetActive(true);

                    //Track the save file
                    _currentSaveFile = 1;
                }
                else
                    LoadData(1);
                break;

            case "File3Button":
                if (_existingSaveFiles[2] == 0)
                {
                    // FileMenu GameObject
                    EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);

                    // FileCreationMenu GameObject
                    EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.transform.parent.Find("FileCreationMenu").gameObject.SetActive(true);

                    //Track the save file
                    _currentSaveFile = 2;
                }
                else
                    LoadData(2);
                break;

            case "ConfirmButton":
                string input = EventSystem.current.currentSelectedGameObject.transform.parent.GetComponentInChildren<TMP_InputField>().text;
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
                    Debug.Log("Hello " + input);
                    EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.transform.parent.Find("FileMenu").gameObject.SetActive(true);
                    EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
                    _playerName = input;
                    Debug.Log("Player name is " + _playerName);

                    CreateData(_currentSaveFile);
                    break;
                }
        }
    }

    public void CheckAllFiles()
    {
        for (int i = 0; i < 3; i++)
        {
            if (File.Exists(Application.persistentDataPath + "/playerInfo" + i + ".dat"))
            {
                _existingSaveFiles[i] = 1;
                GameObject.Find("FileMenu").transform.Find("SaveFile" + (i + 1) + "Name").GetComponent<TMP_Text>().text = DisplayName(i);
            }
            else
            {
                _existingSaveFiles[i] = 0;
                GameObject.Find("FileMenu").transform.Find("SaveFile" + (i + 1) + "Name").GetComponent<TMP_Text>().text = "[BLANK]";
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