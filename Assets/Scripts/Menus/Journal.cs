using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        QuestManager.Instance.ClearCurrentQuest();
    }
}
