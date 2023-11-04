using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class IcefishingHole : MonoBehaviour
{
    public GameObject fishPrefab;
    public Player player;
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
        fishingSoundSource.volume = 0.3f;
        fishingSoundSource.Stop();

        GameManager.instance.pauseEvent += fishingSoundSource.Pause;
        GameManager.instance.unpauseEvent += fishingSoundSource.UnPause;
    }

    void Update()
    {

        if (isPlayerInside)
        {
            if (isFishing)
            {
                chargingBar.UpdateBar((Time.time - fishingStartTime) / player.fishingTime);

                // V�rifiez si le joueur a p�ch� pendant suffisamment de temps.
                if (isFishing && Time.time - fishingStartTime >= player.fishingTime)
                {
                    CaptureFish();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E)||Input.GetKeyDown(KeyCode.T))
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
