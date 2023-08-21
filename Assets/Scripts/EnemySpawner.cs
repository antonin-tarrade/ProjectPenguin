using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    public GameObject enemyPrefab;

    // Nombre d'enemi par spawn
    public int numberOfEnemies;

    // Moyene du cercle d'apparition
    public float spawnRadius;
    // Incertitude du cercle de Spawn 
    public float spawnRandom;

    // Délai entre les spawns individuel
    public float maxSpawnDelay;

    public int waveNumber = 0;
    public int remainingEnemies = 0;
    public int waveDelay = 30;
    public TextMeshProUGUI timer;
    public GameObject timerGO;

    private void Awake()
    {
        instance = this;
    }

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
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemy.GetComponent<Enemy>().spawner = this;
            remainingEnemies++;
            // Delai
            float spawnDelay = Random.Range(0f, maxSpawnDelay);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    // On notifie le spawner qu'un enemi est mort, pour garder le compte des enemis encore en vie
    // et connaître l'avancement d'une vague d'enemis
    public void NotifyDeath()
    {
        remainingEnemies--;
        Debug.Log("Remaining Enemies : " + remainingEnemies);
        if (remainingEnemies == 0)
        {
            waveNumber++;
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
