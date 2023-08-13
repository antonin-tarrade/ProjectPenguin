using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemySpawner : MonoBehaviour
{
    public GameObject ennemyPrefab; 

    // Nombre d'ennemi par spawn
    public int numberOfEnemies;

    // Moyenne du cercle d'apparition
    public float spawnRadius;
    // Incertitude du cercle de Spawn 
    public float spawnRandom;

    // Délai entre les spawns individuel
    public float maxSpawnDelay;

    void Start()
    {
        StartCoroutine(SpawnEnemiesWithDelay());
    }

    private void Update() {

        // Input de test à retirer
        if (Input.GetKeyDown ("p"))
		{
            StartCoroutine(SpawnEnemiesWithDelay());
		}
        
    }

    IEnumerator SpawnEnemiesWithDelay()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Position 
            float angle = Random.Range(0f, Mathf.PI * 2);
            float radius = spawnRadius + Random.Range(-spawnRandom, spawnRandom);
            Vector3 spawnPosition = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

            // Spawn
            Instantiate(ennemyPrefab, spawnPosition, Quaternion.identity);

            // Delai
            float spawnDelay = Random.Range(0f, maxSpawnDelay);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
