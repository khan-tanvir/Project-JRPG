using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

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

    // Start is called before the first frame update
    void Awake()
    {
        if (gameData == null)
        {
            DontDestroyOnLoad(gameObject);
            gameData = this;
        }
        else if (gameData != this)
            Destroy(gameData);
    }

    public void SaveData()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        // This stores it within the appdata folder
        FileStream fileStream = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        // Store the data to the PlayerData class
        PlayerData playerData = new PlayerData();
        playerData.PlayerName = PlayerName;
        playerData.PlayerPosition = PlayerPosition;
        playerData.PlayerQuestProgress = PlayerQuestProgress;

        // Serialise the data
        binaryFormatter.Serialize(fileStream, playerData);

        fileStream.Close();
    }

    public void LoadData()
    {
        // Check if file exists before trying to read it
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

            // Cast the file data as PlayerData before we initialise it to data
            PlayerData data = (PlayerData)binaryFormatter.Deserialize(fileStream);

            fileStream.Close();

            // Load the variables
            PlayerName = data.PlayerName;
            PlayerPosition = data.PlayerPosition;
            PlayerQuestProgress = data.PlayerQuestProgress;
        }
    }
}

[Serializable]
class PlayerData
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

