using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public GameObject player;
    public float playerHealth;
    public GameObject healthUI;
    public GameObject healthPrefab;

    public Vector2 firstHeartPosition;
    public float distanceBetweenHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;


    // Initialise the healthbar UI
    public void InitHealthUI(int numOfHearts)
    {
        hearts = new Image[numOfHearts];
        for (int i=0; i<numOfHearts; i++){
            hearts[i] = Instantiate(healthPrefab).GetComponent<Image>();
            hearts[i].transform.SetParent(healthUI.transform);
            hearts[i].rectTransform.anchoredPosition = firstHeartPosition + new Vector2(distanceBetweenHearts*i, 0);
            //Adjust Scale
            hearts[i].rectTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

    }

    // Update the healthbar UI
    public void UpdateHealthUI(int numOfHearts,float health)
    {
	    float eps = 0.1f;
        for (int i=0; i<hearts.Length; i++){
            if (i + 0.5f + eps > health && i + 0.5f - eps < health) {
                hearts[i].sprite = halfHeart;
            } else if (i < health){
                hearts[i].sprite = fullHeart;
            } else {
                hearts[i].sprite = emptyHeart;
            }

            if (i < numOfHearts){
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }
        }
        
    }

    
}
