using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcefishingHole : MonoBehaviour
{
    public GameObject fishPrefab;
    //public GameObject iceHole;
    public float fishingTime = 10f;
    private bool isFishing = false;
    public Vector3 spawnOffset;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FishCoroutine());  
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    // public void Fish()
    // {
    //     if(!isFishing)
    //     {
    //         isFishing = true;
    //         StartCoroutine(FishCoroutine());
    //     }
    // }

    IEnumerator FishCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(fishingTime);
            Instantiate(fishPrefab, transform.position + spawnOffset , Quaternion.identity);
        }
    }
}
