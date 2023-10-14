using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    public enum Sfx { Hit, PickUp, Shoot }

    public AudioClip[] sfxList;
    private AudioSource backgroundMusicSource;
    private AudioSource soundEffectSource;

    public AudioClip[] fishingSounds;

    [SerializeField] private AudioClip backgroundMusicClip;

    private void Awake()
    {
        instance = this;

        backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        backgroundMusicSource.clip = backgroundMusicClip;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.playOnAwake = true;
        backgroundMusicSource.volume = 0f;

        soundEffectSource = gameObject.AddComponent<AudioSource>();
        soundEffectSource.clip = null;
        soundEffectSource.loop = false;
        soundEffectSource.playOnAwake = true;
        soundEffectSource.volume = 0.2f;
    }

    private void Start()
    {
        sfxList = Resources.LoadAll<AudioClip>("SFX");
        backgroundMusicSource.Play();
    }

    public void PlaySfx(Sfx sfx)
    {
        PlaySfxAtPoint(sfx, Camera.main.gameObject.transform.position);
    }

    public void PlaySfxAtPoint(Sfx sfx, Vector3 point)
    {
        AudioSource.PlayClipAtPoint(sfxList[(int)sfx], point);
    }

    public void PlayFishingSound(int number)
    {
        soundEffectSource.Stop();
        soundEffectSource.PlayOneShot(fishingSounds[number]);
    }

    public void StopEffectSound()
    {
        soundEffectSource.Stop();
    }
}
