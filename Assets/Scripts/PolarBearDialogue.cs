using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PolarBearDialogue : MonoBehaviour
{
    [SerializeField]
    private GameObject ePrompt;
    [SerializeField]
    private GameObject dialogue;
    private bool dialogueActive;
    [SerializeField] TMP_Text tmpDiag;
    [SerializeField] Player player;

    private void Start()
    {
    	ChangerTexte("You have yet to deserve my power");
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
    
    public void ChangerTexte(string newText)
    {
    	tmpDiag.text = newText;
    	
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
        	if (player.score > 25)
       		//if (GameManager.instance.battleData.playerStats.Dmg > 3)
       		{	
       			if (player.GetHasBouclier())
       			{
       				ChangerTexte("Bravo tu as trouvé le bouclier ! tu peux l'activer en appuyant sur 'C', mais attention à ne pas trop l'utiliser...");
       			} else {
       				ChangerTexte("Après avoir autant progressé en attaque, il est désormais tant d'apprendre à vous défendre aussi ! Aller donc au nord-est de cette ville, on dit qu'un navire de guerre s'y était échoué il y a fort fort longtemps, peut être y trouverez vous encore un bouclier...");
       			}
       		}
       		ShowPrompt();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) HidePrompt();

        if (dialogueActive) HideDialogue();
    }
}
