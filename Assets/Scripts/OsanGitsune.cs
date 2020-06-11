using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OsanGitsune : MonoBehaviour
{
    public Button option1;

    public Button option2;

    private bool choiceMade;

    private DialogueManager dialogueManager;

    private int i;

    private int correctAnswers;

    private bool InProgress;

    private bool riddle1Answered;

    private bool riddle2Answered;

    private bool riddle3Answered;

    private bool riddle4Answered;

    private bool riddle5Answered;

    private bool riddle6Answered;

    private DialogueTrigger trigger;

    // Start is called before the first frame update
    void Start()
    {
        i = 1;
        correctAnswers = 0;
        riddle1Answered = false;
        riddle2Answered = false;
        riddle3Answered = false;
        riddle4Answered = false;
        riddle5Answered = false;
        riddle6Answered = false;
        choiceMade = false;
        //InProgress = true;
        trigger = gameObject.GetComponent<DialogueTrigger>();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    public void RiddleStart()
    {
        if (i == 1)
        {
            if (riddle1Answered == false)
            {
                dialogueManager.script = "Riddles" + i;
                trigger.dialogue.script = dialogueManager.script;
                trigger.TriggerDialogue();
            }
        }



            if (i == 2)
            {
                if (riddle2Answered == false)
                {
                dialogueManager.script = "Riddles" + i;
                trigger.dialogue.script = dialogueManager.script;
                trigger.TriggerDialogue();
                }
                else
                {
                    i = Random.Range(3, 7);
                }
            }

            if (i == 3)
            {
                if (riddle3Answered == false)
                {
                dialogueManager.script = "Riddles" + i;
                trigger.dialogue.script = dialogueManager.script;
                trigger.TriggerDialogue();
                }
                else
                {
                    i = Random.Range(4, 7);
                }
            }

            if (i == 4)
            {
                if(riddle4Answered == false)
                {
                dialogueManager.script = "Riddles" + i;
                trigger.dialogue.script = dialogueManager.script;
                trigger.TriggerDialogue();
                }
                else
                {
                    i = Random.Range(5, 7);
                }
            }

            if (i == 5)
            {
                 if (riddle5Answered == false)
                 {
                 dialogueManager.script = "Riddles" + i;
                 trigger.dialogue.script = dialogueManager.script;
                trigger.TriggerDialogue();
                 }
                 else
                 {
                    i = 6;
                 }
            }

            if (i == 6)
            {
                if (riddle6Answered == false)
                {
                    dialogueManager.script = "Riddles" + i;
                    trigger.dialogue.script = dialogueManager.script;
                    trigger.TriggerDialogue();
                }
                else
                {
                    if(riddle2Answered==false)
                    {
                    i = 2;
                    dialogueManager.script = "Riddles" + i;
                    trigger.dialogue.script = dialogueManager.script;
                    trigger.TriggerDialogue();
                    }

                    else if (riddle3Answered == false)
                    {
                    i = 3;
                    dialogueManager.script = "Riddles" + i;
                    trigger.dialogue.script = dialogueManager.script;
                    trigger.TriggerDialogue();
                    }

                    else if (riddle4Answered == false)
                    {
                    i = 4;
                    dialogueManager.script = "Riddles" + i;
                    trigger.dialogue.script = dialogueManager.script;
                    trigger.TriggerDialogue();
                    }

                    else if (riddle5Answered == false)
                    {
                    i = 5;
                    dialogueManager.script = "Riddles" + i;
                    trigger.dialogue.script = dialogueManager.script;
                    trigger.TriggerDialogue();
                    }

                    else
                    {
                    dialogueManager.script = "OsanFinal";
                    trigger.dialogue.script = dialogueManager.script;
                    trigger.TriggerDialogue();
                    }
                }
            }
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueManager.script == "Riddles" + i)
        {
            if (choiceMade == false)
            {
                option2.onClick.AddListener(WrongAnswer);
                option1.onClick.AddListener(RightAnswer);
                choiceMade = true;
            }
        }
    }

    private void RightAnswer()
    {
        correctAnswers += 1;
        if (correctAnswers >= 3)
        {
            //open the way
        }
        if(i==1)
        {
            riddle1Answered = true;
        }
        else if (i == 2)
        {
            riddle2Answered = true;
        }
        else if (i == 3)
        {
            riddle3Answered = true;
        }
        else if (i == 4)
        {
            riddle4Answered = true;
        }
        else if (i == 5)
        {
            riddle5Answered = true;
        }
        else if (i == 6)
        {
            riddle6Answered = true;
        }
        i = Random.Range(2, 7);
    }

    private void WrongAnswer()
    {
        if (i == 1)
        {
            riddle1Answered = true;
        }
        i = Random.Range(2, 7);
    }
}
