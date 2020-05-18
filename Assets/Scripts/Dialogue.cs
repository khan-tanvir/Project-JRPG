using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    public string name;

    public Sprite[] sprite;

    [TextArea(3,10)]
    public string[] lines;
}
