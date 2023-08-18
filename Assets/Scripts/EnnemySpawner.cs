using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public int remainingEnnemies = 0;
    public int waveDelay = 30;
    public TextMeshProUGUI timer;
    public GameObject timerGO; 
    void Start()
    {
        StartCoroutine(SpawnEnemiesWithDelay());
    }

    private void Update()
    {

        // Input de test à retirer
        if (Input.GetKeyDown("p"))
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
            GameObject ennemy = Instantiate(ennemyPrefab, spawnPosition, Quaternion.identity);
            ennemy.GetComponent<Ennemy>().spawner = this;
            remainingEnnemies++;
            // Delai
            float spawnDelay = Random.Range(0f, maxSpawnDelay);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void NotifyDeath()
    {
        remainingEnnemies--;
        Debug.Log("Remaining Enemies : " + remainingEnnemies);
        if (remainingEnnemies == 0)
        {
            timerGO.SetActive(true);
            StartCoroutine(WaitForNextWave());
        }
    }

    private IEnumerator WaitForNextWave()
    {
        for (int i = waveDelay; i > 0; i--)
        {
            timer.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        timerGO.SetActive(false);
        StartCoroutine(SpawnEnemiesWithDelay());
    }
}
