using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    private bool dialogueTriggered = false;

    public void TriggerDialogue()
    {
        dialogueTriggered = FindObjectOfType<DialogueManager>().DialogueStarted();
        if (dialogueTriggered == false)
        {
            dialogue.assignLines();
            FindObjectOfType<DialogueManager>().StartConversation(dialogue);
        }
    }
}
