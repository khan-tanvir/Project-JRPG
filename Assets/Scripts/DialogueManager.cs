using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Animator animator;

    public Animator choiceAnimator;

    private GameObject box;

    private GameObject optionBox;

    private Queue<string> lines;

    private string[] previousLines;

    private bool ConversationStarted;

    private bool optionsPresent;

    public Text nameText;

    public string script;

    public Text dialogueText;

    public Image sprite;

    private int lineCount;

    private int prevLineCount;

    private Dialogue tempDialogueCheck;

    // Start is called before the first frame update
    void Start()
    {
        lineCount = 0;
        optionsPresent = false;
        ConversationStarted = false;
        optionBox = GameObject.Find("Option selecter");
        box = GameObject.Find("Dialogue Box");
        lines = new Queue<string>();
    }

    public void optionsInDialogue(bool InDialogue)
    {
        optionsPresent = InDialogue;
    }

    public void StartConversation(Dialogue dialogue)
    {
        lineCount = 0;

        tempDialogueCheck = dialogue;

        script = dialogue.script;

        ConversationStarted = true;

        box.SetActive(true);

        animator.SetBool("isActive", true);

        nameText.text = dialogue.name[lineCount];

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
            if (optionsPresent == false)
            {
                EndDialogue();
            }
            else
            {
                choiceAnimator.SetBool("IsActive", true);
            }
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
        Invoke("deactivate", 0.2f);
        ConversationStarted = false;
    }

    public void deactivate()
    {
        box.SetActive(false);
    }

    public bool DialogueStarted()
    {
        return ConversationStarted;
    }

}
