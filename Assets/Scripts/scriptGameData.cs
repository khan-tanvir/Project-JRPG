using System;
using System.Collections;
using System.Collections.Generic;
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

    // Hold reference to this object
    private static scriptGameData gameData;

    [SerializeField]
    private int numberOfInventorySlots = 25;

    [SerializeField]
    private PlayerData _playerData = null;

    // Track if save file exists
    private int[] _existingSaveFiles = { 0, 0, 0 };

    // Track current save file
    [SerializeField]
    private int _currentSaveFile;

    public static scriptGameData GameDataManager
    {
        get { return gameData; }
    }

    public int CurrentSaveFile
    {
        get { return _currentSaveFile; }
        set { _currentSaveFile = value; }
    }

    public string PlayerName
    {
        get { return _playerData.PlayerName; }
        set { _playerData.PlayerName = value; }
    }

    public float[] PlayerPosition
    {
        get { return _playerData.PlayerPosition; }
        set { _playerData.PlayerPosition = value; }
    }

    public int PlayerQuestProgress
    {
        get { return _playerData.PlayerQuestProgress; }
        set { _playerData.PlayerQuestProgress = value; }
    }

    public int[] ExistingSaveFiles
    {
        get { return _existingSaveFiles; }
    }

    public List<InventoryItem> InventoryItems
    {
        get { return _playerData.InventoryItems; }
        set { _playerData.InventoryItems = value; }
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

        ClearData();
    }

    private void Start()
    {
        if (PlayerPrefs.GetFloat("Master Volume", 0.8f) < 0.0001f || float.IsNaN(PlayerPrefs.GetFloat("Master Volume")))
            PlayerPrefs.SetFloat("Master Volume", 0.8f);

        if (PlayerPrefs.GetFloat("Music Volume", 0.8f) < 0.0001f || float.IsNaN(PlayerPrefs.GetFloat("Music Volume")))
            PlayerPrefs.SetFloat("Music Volume", 0.8f);
    }

    public void CreateData(string name)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.persistentDataPath + "/playerInfo" + _currentSaveFile + ".dat");

        // Only storing the name because this is called once after every save file creation
        PlayerData temp = new PlayerData();
        temp.PlayerName = name;

        // Serialise the data
        binaryFormatter.Serialize(fileStream, temp);

        fileStream.Close();

        Debug.Log("Created save file");
        CheckAllFiles();
    }

    public void SaveData()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        // This stores it within the appdata folder
        FileStream fileStream = File.Create(Application.persistentDataPath + "/playerInfo" + _currentSaveFile + ".dat");

        Debug.Log("SAVING " + PlayerName);
        Debug.Log("SAVING " + PlayerPosition);
        Debug.Log("SAVING " + PlayerQuestProgress);

        // Serialise the data
        binaryFormatter.Serialize(fileStream, _playerData);

        fileStream.Close();

        Debug.Log("Player data has been saved");
    }

    public void LoadData(int pos)
    {
        ClearData();
        
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Open(Application.persistentDataPath + "/playerInfo" + pos + ".dat", FileMode.Open);

        // Cast the file data as PlayerData before we initialise it to data
        PlayerData data = (PlayerData)binaryFormatter.Deserialize(fileStream);

        fileStream.Close();

        // Load the variables
        _playerData = data;

        //Track the save file
        _currentSaveFile = pos;

        Debug.Log("LOADING " + PlayerName);
        Debug.Log("LOADING " + PlayerPosition);
        Debug.Log("LOADING " + PlayerQuestProgress);

        Debug.Log("Loading Save File " + pos);

        //GameObject.Find("Canvas").gameObject.SetActive(false);

        SceneManager.LoadScene(SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/Game.unity"));
        scriptAudioManager.audioManager.StopCurrentMusic();
    }

    public void CheckAllFiles()
    {
        Transform fileMenu = GameObject.Find("Canvas").transform.Find("File Menu").transform;

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
    }

    private string DisplayName(int pos)
    {
        // Get the player name of the file
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Open(Application.persistentDataPath + "/playerInfo" + pos + ".dat", FileMode.Open);

        // Cast the file data as PlayerData before we can do something with it
        PlayerData data = (PlayerData)binaryFormatter.Deserialize(fileStream);

        fileStream.Close();

        string nameToReturn = data.PlayerName;
        return nameToReturn;
    }

    public void ClearData()
    {
        // This function will be called when the player goes back to main menu from the game scene
        _playerData = null;
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
public class PlayerData
{
    [SerializeField]
    private string _playerName;

    [SerializeField]
    private float[] _position = { -999.0f, -999.0f };

    [SerializeField]
    private int _questProgress;

    private List<InventoryItem> _inventoryItems = null;

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

    public List<InventoryItem> InventoryItems
    {
        get { return _inventoryItems; }
        set { _inventoryItems = value; }
    }
}

[Serializable]
public class InventoryItem
{
    private int _itemID;

    private int _position;

    public int itemID
    {
        get { return _itemID; }
        set { _itemID = value; }
    }

    public int Position
    {
        get { return _position; }
        set { _position = value; }
    }

    public InventoryItem(int ID, int position)
    {
        _itemID = ID;
        _position = position;
    }
}