using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialogue : MonoBehaviour
{

    public GameObject dialogBox;
    public bool playerInRange;
    public Dialogue dialogue;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void TriggerDialogue()
    {
        dialogBox.SetActive(true);
        dialogue.assignLines();
        FindObjectOfType<DialogueManager>().StartConversation(dialogue);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerInRange)
        {
            //if (dialogBox.activeInHierarchy)
            //{
            //    dialogBox.SetActive(false);
            //}
            //else
            //{
            //    dialogBox.SetActive(true);
            //    TriggerDialogue();
            //}

            TriggerDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("Player in Range");
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("Player Left Range");
            playerInRange = false;
            dialogBox.SetActive(false);
        }
    }
}
