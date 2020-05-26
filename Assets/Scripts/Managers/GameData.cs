using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [SerializeField]
    private float[] _position = { -999.0f, -999.0f };

    [SerializeField]
    private int _questProgress;

    private List<InventoryItem> _inventoryItems = null;

    public string PlayerName
    {
        get;
        set;
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
        get;
        set;
    }
}

[System.Serializable]
public class InventoryItem
{
    public int ID
    {
        get;
        internal set;
    }

    public int Position
    {
        get;
        internal set;
    }

    public InventoryItem(int itemID, int pos)
    {
        ID = itemID;
        Position = pos;
    }
}

public class GameData : MonoBehaviour
{
    public PlayerData PlayerData
    {
        get;
        set;
    }

    public int CurrentSaveFile
    {
        get;
        set;
    }

    public int[] ExistingSaveFiles
    {
        get;
        internal set;
    }

    public float MasterVolume
    {
        get { return PlayerPrefs.GetFloat("Master Volume", 0.42f); }
    }

    public float MusicVolume
    {
        get { return PlayerPrefs.GetFloat("Music Volume", 0.18f); }
    }

    public static GameData Instance
    {
        get;
        internal set;
    }

    // Start is called before the first frame update
    private void Awake()
    {
        CreateInstance();
        ClearData();

        ExistingSaveFiles = new int[3];
    }


    private void CreateInstance()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void CreateData(string name)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.persistentDataPath + "/playerInfo" + CurrentSaveFile + ".dat");

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
        FileStream fileStream = File.Create(Application.persistentDataPath + "/playerInfo" + CurrentSaveFile + ".dat");

        Debug.Log("SAVING " + PlayerData.PlayerName + " to File " + CurrentSaveFile);

        // Serialise the data
        binaryFormatter.Serialize(fileStream, PlayerData);

        fileStream.Close();

        QuestsDatabase database = new QuestsDatabase();

        database.SaveToDatabase(CurrentSaveFile, QuestManager.Instance.Quests);

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
        PlayerData = data;

        //Track the save file
        CurrentSaveFile = pos;

        Debug.Log("LOADING " + PlayerData.PlayerName + " from File " + CurrentSaveFile);

        SceneManagerScript.Instance.SceneToGoTo("Game");
    }

    public void CheckAllFiles()
    {
        Transform fileMenu = GameObject.Find("Canvas").transform.Find("File Menu").transform;

        for (int i = 0; i < 3; i++)
        {
            TMP_Text temp = fileMenu.Find("File " + (i + 1) + " Text").GetComponent<TMP_Text>();

            if (File.Exists(Application.persistentDataPath + "/playerInfo" + i + ".dat"))
            {
                ExistingSaveFiles[i] = 1;
                temp.text = DisplayName(i);
            }
            else
            {
                ExistingSaveFiles[i] = 0;
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

        return data.PlayerName;
    }

    public void ClearData()
    {
        // This function will be called when the player goes back to main menu from the game scene
        PlayerData = null;
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