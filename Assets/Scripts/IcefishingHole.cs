using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcefishingHole : MonoBehaviour
{
    public GameObject fishPrefab;
    public float fishingTime = 10f;
    private bool isFishing = false;
    private bool isPlayerInside = false;
    private float fishingStartTime = 0f;
    public Vector3 spawnOffset;

    [SerializeField] private ChargingBar chargingBar;

    void Update()
    {
        if (isPlayerInside)
        {
            if (isFishing)
            {
                chargingBar.UpdateBar((Time.time - fishingStartTime) / fishingTime);

                // Vérifiez si le joueur a pêché pendant suffisamment de temps.
                if (isFishing && Time.time - fishingStartTime >= fishingTime)
                {
                    CaptureFish();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    StartFishing();
                    chargingBar.SetChargeOn();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            StopFishing();
        }
    }

    void StartFishing()
    {
        isFishing = true;
        fishingStartTime = Time.time;
        AudioManager.instance.PlayFishingSound(0);
    }

    void StopFishing()
    {
        isFishing = false;
        chargingBar.ResetBar();
        AudioManager.instance.StopEffectSound();

    }

    void CaptureFish()
    {
        isFishing = false;
        Instantiate(fishPrefab, transform.position + spawnOffset, Quaternion.identity);
        AudioManager.instance.PlayFishingSound(1);
        chargingBar.ResetBar();


    }
}
