using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scriptIchiNiSanTrap : MonoBehaviour
{
    private DialogueManager dialogueManager;

    public Button option1;

    public Button option2;

    private bool choiceMade;

    private SpriteRenderer rend;

    void Start()
    {
        choiceMade = false;
        dialogueManager = FindObjectOfType<DialogueManager>();
        rend = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (dialogueManager.script == "INSTrap")
        {
            if (choiceMade == false)
            {
                option2.onClick.AddListener(LetPlayerPass);
                option1.onClick.AddListener(KillPlayer);
                choiceMade = true;
            }
        }
    }

    private void KillPlayer()
    {
        //kills the player
        Debug.Log("You are dead!");
    }

    private void LetPlayerPass()
    {
        //lets the player pass
        StartCoroutine("Fade");
        Debug.Log("You may pass");
    }

    IEnumerator Fade()
    {
        for(float f = 1f;f>=-0.05f;f-=0.05f)
        {
            Color c = rend.material.color;
            c.a = f;
            rend.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
