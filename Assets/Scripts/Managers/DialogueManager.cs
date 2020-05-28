using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    #region Private Fields

    private int lineCount;

    private Queue<string> lines;

    private string[] previousLines;

    private int prevLineCount;

    private Dialogue tempDialogueCheck;

    #endregion Private Fields

    #region Public Fields

    //public Animator animator;
    public GameObject dialogueBox;

    public TMP_Text dialogueText;

    public TMP_Text nameText;

    public Image sprite;

    #endregion Public Fields

    #region Private Methods

    // Start is called before the first frame update
    private void Start()
    {
        lineCount = 0;
        lines = new Queue<string>();
    }

    private IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            dialogueText.color = Color.black;
            yield return null;
        }
    }

    #endregion Private Methods

    #region Public Methods

    public void DisplayText()
    {
        if (lines.Count == 0)
        {
            lineCount = 0;
            EndDialogue();
            return;
        }
        else
        {
            nameText.text = tempDialogueCheck.name[lineCount];
            sprite.sprite = tempDialogueCheck.sprite[lineCount];
            lineCount += 1;
            prevLineCount = lineCount - 1;
        }
        string line = lines.Dequeue();
        previousLines[lineCount - 1] = line;
        StopAllCoroutines();
        StartCoroutine(TypeLine(line));
    }

    public void EndDialogue()
    {
        //animator.SetBool("isActive", false);
        dialogueBox.SetActive(false);
    }

    public void ForwardText()
    {
        if (prevLineCount + 1 == lineCount)
        {
            dialogueText.color = Color.black;
            return;
        }
        else
        {
            prevLineCount += 1;
            nameText.text = tempDialogueCheck.name[prevLineCount];
            sprite.sprite = tempDialogueCheck.sprite[prevLineCount];
            dialogueText.text = previousLines[prevLineCount];
            dialogueText.color = Color.blue;
        }
    }

    public void PreviousText()
    {
        if (prevLineCount == 0)
        {
            return;
        }
        else
        {
            prevLineCount -= 1;
            nameText.text = tempDialogueCheck.name[prevLineCount];
            sprite.sprite = tempDialogueCheck.sprite[prevLineCount];
            dialogueText.text = previousLines[prevLineCount];
            dialogueText.color = Color.blue;
        }
    }

    public void StartConversation(Dialogue dialogue)
    {
        tempDialogueCheck = dialogue;

        dialogueBox.SetActive(true);

        nameText.text = dialogue.name[lineCount];

        lineCount = 0;

        sprite.sprite = dialogue.sprite[lineCount];

        lines.Clear();

        foreach (string line in dialogue.lines)
        {
            lines.Enqueue(line);
        }
        previousLines = new string[lines.Count];
        DisplayText();
    }

    #endregion Public Methods
}