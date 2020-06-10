using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    #region Private Fields

    private DialogueManager dialogueManager;

    private bool dialogueTriggered = false;

    private GameObject option;

    private GameObject optionBox;

    private bool optionsAvailable;

    #endregion Private Fields

    #region Public Fields

    public Dialogue dialogue;

    #endregion Public Fields

    #region Private Methods

    private void Start()
    {
        optionBox = GameObject.Find("Option selecter");
        optionsAvailable = false;
        option = gameObject;
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    #endregion Private Methods

    #region Public Methods

    public void TriggerChoice()
    {
        dialogueTriggered = FindObjectOfType<DialogueManager>().DialogueStarted();
        {
            dialogue.script = dialogueManager.script;
            dialogue.option = option.name;
            dialogue.AssignOptions();
            optionsAvailable = false;
            dialogueManager.optionsInDialogue(optionsAvailable);
            dialogueManager.StartConversation(dialogue);
            dialogueManager.choiceAnimator.SetBool("IsActive", false);
        }
    }

    public void TriggerDialogue()
    {
        dialogueTriggered = FindObjectOfType<DialogueManager>().DialogueStarted();
        if (!dialogueTriggered)
        {
            optionsAvailable = dialogue.CheckForOptions();
            dialogueManager.optionsInDialogue(optionsAvailable);
            dialogue.AssignSpeechLines();
            dialogueManager.StartConversation(dialogue);
        }
    }

    #endregion Public Methods
}