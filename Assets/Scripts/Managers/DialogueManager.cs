using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    #region Private Fields

    [SerializeField]
    private GameObject _dialoguePanel;

    private bool ConversationStarted;

    private int lineCount;

    private Queue<string> lines;

    private bool optionsPresent;

    private string[] previousLines;

    private int prevLineCount;

    private Dialogue tempDialogueCheck;

    #endregion Private Fields

    #region Public Fields

    public GameObject _optionsBox;

    public Animator animator;

    public Animator choiceAnimator;

    public TMP_Text dialogueText;

    public TMP_Text nameText;

    public string script;

    public Image sprite;

    #endregion Public Fields

    #region Public Properties

    public static DialogueManager Instance
    {
        get;
        internal set;
    }

    #endregion Public Properties

    #region Private Methods

    private void Awake()
    {
        CreateInstance();
    }

    private void CreateInstance()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        lineCount = 0;
        optionsPresent = false;
        ConversationStarted = false;
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

    public void Deactivate()
    {
        _dialoguePanel.SetActive(false);
        _optionsBox.SetActive(false);
    }

    public bool DialogueStarted()
    {
        return ConversationStarted;
    }

    public void DisplayText()
    {
        if (lines.Count == 0)
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
        animator.SetBool("isActive", false);
        Invoke("Deactivate", 0.2f);
        ConversationStarted = false;
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

    public void optionsInDialogue(bool InDialogue)
    {
        optionsPresent = InDialogue;
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
        lineCount = 0;

        tempDialogueCheck = dialogue;

        script = dialogue.script;

        ConversationStarted = true;

        _dialoguePanel.SetActive(true);

        animator.SetBool("isActive", true);

        nameText.text = dialogue.name[lineCount];

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