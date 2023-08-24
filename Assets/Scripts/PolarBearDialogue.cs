using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolarBearDialogue : MonoBehaviour
{
    [SerializeField]
    private GameObject ePrompt;
    [SerializeField]
    private GameObject dialogue;
    private bool dialogueActive;

    private void Start()
    {
        HideDialogue();
        HidePrompt();
    }


    public void ShowPrompt()
    {
        ePrompt.SetActive(true);
    }

    public void HidePrompt()
    {
        ePrompt.SetActive(false);
    }

    public void ShowDialogue()
    {
        dialogue.SetActive(true);
        dialogueActive = true;
        HidePrompt();
    }

    public void HideDialogue()
    {
        dialogue.SetActive(false);
        dialogueActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) ShowPrompt();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) HidePrompt();

        if (dialogueActive) HideDialogue();
    }
}
