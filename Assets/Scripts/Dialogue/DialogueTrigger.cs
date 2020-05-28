using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    #region Public Fields

    public Dialogue dialogue;

    #endregion Public Fields

    #region Public Methods

    public void TriggerDialogue()
    {
        dialogue.assignLines();
        FindObjectOfType<DialogueManager>().StartConversation(dialogue);
    }

    #endregion Public Methods
}