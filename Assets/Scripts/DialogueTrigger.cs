using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    private GameObject option;

    private GameObject optionBox;

    private bool optionsAvailable;

    private DialogueManager dialogueManager;

    private bool dialogueTriggered = false;

    void Start()
    {
        optionBox = GameObject.Find("Option selecter");
        optionsAvailable = false;
        option = gameObject;
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    public void TriggerDialogue()
    {
        dialogueTriggered = FindObjectOfType<DialogueManager>().DialogueStarted();
        if (dialogueTriggered == false)
        {
            optionsAvailable = dialogue.CheckForOptions();
            dialogueManager.optionsInDialogue(optionsAvailable);
            dialogue.assignLines();
            dialogueManager.StartConversation(dialogue);
        }
    }
    public void TriggerChoice()
    {
        dialogueTriggered = FindObjectOfType<DialogueManager>().DialogueStarted();
        {
            dialogue.script = dialogueManager.script;
            dialogue.option = option.name;
            dialogue.assignLinesForOptions();
            optionsAvailable = false;
            dialogueManager.optionsInDialogue(optionsAvailable);
            dialogueManager.StartConversation(dialogue);
            dialogueManager.choiceAnimator.SetBool("IsActive", false);
        }
    }
}
