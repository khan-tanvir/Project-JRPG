using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;

public class scriptGameData : MonoBehaviour
{
    // Saving and Loading Data
    // Singleton Class

    // Hold reference to this object
    public static scriptGameData gameData;

    // Variables to save
    public float name;
    public float health;
    public float experience;
    public float[] position = new float[3];
    
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
}
