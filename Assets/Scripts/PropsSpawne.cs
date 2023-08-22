using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PropsSpawne : MonoBehaviour
{
    public GameObject iceBlockPrefab; 

    // Nombre de props
    public int numberIceBlock;

    // Zone de Spawn
    public float spawnRadius;
    public float minSpawn = 1f;

    void Start()
    {
        // Spawn aléatoire (simpliste, pas de prise en compte de superposition etc etc)
        IceBlockSpawn();
    }

    private void IceBlockSpawn()
    {
        for (int i = 0; i < numberIceBlock; i++)
        {
            // Position 
            float angle = Random.Range(0f, Mathf.PI * 2);
            float radius = Random.Range(minSpawn, spawnRadius);
            Vector3 spawnPosition = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

            // Spawn
            Instantiate(iceBlockPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
