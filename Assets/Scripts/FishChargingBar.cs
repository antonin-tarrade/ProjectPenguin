/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishChargingBar : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject fishUI;
    [SerializeField] private GameObject fishPrefab;

    [SerializeField] private Vector2 firstFishPosition;
    [SerializeField] private float distanceBetweenFishs;

    [SerializeField] private Image[] fishs;
    [SerializeField] private Sprite fullFish;
    [SerializeField] private Sprite halfFish;
    [SerializeField] private Sprite emptyFish;

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image progressImage;

    protected bool isCharging = false;

    // Initialise the healthbar UI
    void Start(int numOfFishs)
    {
        fishs = new Image[numOfFishs];
        for (int i=0; i<numOfFishs; i++){
            fishs[i] = Instantiate(fishPrefab).GetComponent<Image>();
            fishs[i].transform.SetParent(fishUI.transform);
            fishs[i].rectTransform.anchoredPosition = firstFishPosition + new Vector2(distanceBetweenFishs*i, 0);
            //Adjust Scale
            fishs[i].rectTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        
        this.gameObject.SetActive(false);
    }
   
    public void SetChargeOn()
    {
        isCharging = true;
        this.gameObject.SetActive(true);
    }

    public void UpdateBar(float number)
    {
        if (isCharging)
        {
            progressImage.fillAmount = Mathf.Min(1f, number);
        }

        if (number >= 1)
        {
            isCharging = false;
        }
    }

    // Update the healthbar UI
    public void UpdatefishUI(int numOfFishs,float health)
    {
  
        //InitfishUI(numOfFishs);

        for (int i=0; i<fishs.Length; i++){
            if (i + 0.5f == health) {
                fishs[i].sprite = halfFish;
            }else if (i < health){
                fishs[i].sprite = fullFish;
            } else {
                fishs[i].sprite = emptyFish;
            }

            if (i < numOfFishs){
                fishs[i].enabled = true;
            } else {
                fishs[i].enabled = false;
            }
        }
        
    }
}
*/