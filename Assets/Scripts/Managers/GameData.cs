using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;

public class GameData : MonoBehaviour
{
    #region Public Properties

    public static GameData Instance
    {
        get;
        internal set;
    }

    public int CurrentSaveFile
    {
        get;
        set;
    }

    public float EffectVolume
    {
        get => PlayerPrefs.GetFloat("Effects Volume", 0.3f);
    }

    public int[] ExistingSaveFiles
    {
        get;
        internal set;
    }

    public float MasterVolume
    {
        get => PlayerPrefs.GetFloat("Master Volume", 0.42f);
    }

    public float MusicVolume
    {
        get => PlayerPrefs.GetFloat("Music Volume", 0.18f);
    }

    public PlayerData PlayerData
    {
        get;
        set;
    }

    #endregion Public Properties

    #region Private Methods

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

    #endregion Private Methods

    #region Public Methods

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

    public void ClearData()
    {
        // This function will be called when the player goes back to main menu from the game scene
        PlayerData = null;
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

        CheckAllFiles();
    }

    public void DeleteData(int pos)
    {
        File.Delete(Application.persistentDataPath + "/playerInfo" + pos + ".dat");
        File.Delete(Application.persistentDataPath + "/playerQuestDB" + pos + ".json");
        CheckAllFiles();
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

        if (PlayerData.SceneObjectsList == null)
        {
            PlayerData.SceneObjectsList = new List<SceneObject>();
        }

        SceneManagerScript.Instance.SceneObjects = PlayerData.SceneObjectsList;

        //Track the save file
        CurrentSaveFile = pos;

        FindObjectOfType<Canvas>().transform.Find("Warning Pop-Up").gameObject.SetActive(true);
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

    #endregion Public Methods
}