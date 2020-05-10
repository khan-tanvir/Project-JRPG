using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Animator animator;

    private Queue<string> lines;

    public Text nameText;

    public Text dialogueText;

    // Start is called before the first frame update
    void Start()
    {
        lines = new Queue<string>();
    }

    public void StartConversation(Dialogue dialogue)
    {
        animator.SetBool("isActive",true);

        nameText.text = dialogue.name;

        lines.Clear();

        foreach(string line in dialogue.lines)
        {
            lines.Enqueue(line);
        }
        DisplayText();
    }

    public void DisplayText()
    {
        if(lines.Count==0)
        {
            EndDialogue();
            return;
        }
        string line = lines.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeLine(line));
    }

    IEnumerator TypeLine (string line)
    {
        dialogueText.text = "";
        foreach(char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        animator.SetBool("isActive", false);
    }


}
