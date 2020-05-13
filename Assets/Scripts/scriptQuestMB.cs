using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scriptQuestMB : MonoBehaviour
{

    // Get reference to non-MB 
    public scriptQuest Quest
    {
        get;
        set;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        GetComponent<TMP_Text>().color = Color.yellow;
        scriptQuestManager._questManager.ShowDescription(Quest);
    }

    public void OnDeselect()
    {
        GetComponent<TMP_Text>().color = Color.white;

    }
}
