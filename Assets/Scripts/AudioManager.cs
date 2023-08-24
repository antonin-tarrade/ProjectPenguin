using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    public enum Sfx { Hit, PickUp, Shoot }

    public AudioClip[] sfxList;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        sfxList = Resources.LoadAll<AudioClip>("SFX");
    }

    public void PlaySfx(Sfx sfx)
    {
        PlaySfxAtPoint(sfx, Camera.main.gameObject.transform.position);
    }

    public void PlaySfxAtPoint(Sfx sfx, Vector3 point)
    {
        AudioSource.PlayClipAtPoint(sfxList[(int)sfx], point);
    }
}
