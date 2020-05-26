using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Animator animator;

    private Queue<string> lines;

    private string[] previousLines;

    public Text nameText;

    public Text dialogueText;

    public Image sprite;

    private int lineCount;

    private int prevLineCount;

    private Dialogue tempDialogueCheck;

    // Start is called before the first frame update
    void Start()
    {
        lineCount = 0;
        lines = new Queue<string>();
    }

    public void StartConversation(Dialogue dialogue)
    {
        tempDialogueCheck = dialogue;

        animator.SetBool("isActive", true);

        nameText.text = dialogue.name[lineCount];

        lineCount = 0;

        sprite.sprite = dialogue.sprite[lineCount];

        lines.Clear();

        foreach(string line in dialogue.lines)
        {
            lines.Enqueue(line);
        }
        previousLines = new string[lines.Count];
        DisplayText();
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

    public void ForwardText()
    {
        if (prevLineCount+1 == lineCount)
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

    public void DisplayText()
    {
        if (lines.Count==0)
        {
            lineCount = 0;
            EndDialogue();
            return;
        }
        else
        {
            nameText.text = tempDialogueCheck.name[lineCount];
            sprite.sprite = tempDialogueCheck.sprite[lineCount];
            lineCount +=1;
            prevLineCount = lineCount-1;
        }
        string line = lines.Dequeue();
        previousLines[lineCount-1] = line;
        StopAllCoroutines();
        StartCoroutine(TypeLine(line));
    }

    IEnumerator TypeLine (string line)
    {
        dialogueText.text = "";
        foreach(char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            dialogueText.color = Color.black;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        animator.SetBool("isActive", false);
    }


}
