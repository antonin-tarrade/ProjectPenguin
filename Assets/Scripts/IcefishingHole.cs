using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcefishingHole : MonoBehaviour
{
    public GameObject fishPrefab;
    public GameObject playerObject;
    public Player player;
    private float fishingTime;
    private bool isFishing = false;
    private bool isPlayerInside = false;
    private float fishingStartTime = 0f;
    public Vector3 spawnOffset;

    private AudioSource fishingSoundSource;
    [SerializeField] List<AudioClip> fishingSoundClips;

    [SerializeField] private ChargingBar chargingBar;


    private void Start()
    {

        // Fishing hole
        fishingSoundSource = GetComponent<AudioSource>();
        fishingSoundSource.Stop();
        fishingTime = player.fishingTime;


        GameManager.instance.pauseEvent += fishingSoundSource.Pause;
        GameManager.instance.unpauseEvent += fishingSoundSource.UnPause;
    }

    void Update()
    {
        if (isPlayerInside)
        {
            if (isFishing)
            {
                chargingBar.UpdateBar((Time.time - fishingStartTime) / fishingTime);

                // V�rifiez si le joueur a p�ch� pendant suffisamment de temps.
                if (isFishing && Time.time - fishingStartTime >= fishingTime)
                {
                    CaptureFish();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E)||Input.GetKeyDown(KeyCode.G))
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
        fishingSoundSource.PlayOneShot(fishingSoundClips[0]);
        //AudioManager.instance.PlayFishingSound(0);
    }

    void StopFishing()
    {
        isFishing = false;
        chargingBar.ResetBar();
        fishingSoundSource.Stop();
        //AudioManager.instance.StopEffectSound();

    }

    void CaptureFish()
    {
        isFishing = false;
        Instantiate(fishPrefab, transform.position + spawnOffset, Quaternion.identity);
        fishingSoundSource.Stop();
        fishingSoundSource.PlayOneShot(fishingSoundClips[1]);
        //AudioManager.instance.PlayFishingSound(1);
        chargingBar.ResetBar();


    }
}
