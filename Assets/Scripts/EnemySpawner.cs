using Ennemies;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    public GameObject enemyPrefab;
    public List<GameObject> enemyTrackers;

    // Nombre d'enemi par spawn
    public int numberOfEnemies;

    // Moyene du cercle d'apparition
    public float spawnRadius;
    // Incertitude du cercle de Spawn 
    public float spawnRandom;

    // Délai entre les spawns individuel
    public float maxSpawnDelay;

    public int waveNumber = 0;
    public int totalNumber;
    public WaveData[] waves;
    public int remainingEnemies = 0;
    public int waveDelay = 30;
    public TextMeshProUGUI timer;
    public GameObject timerGO;


    public bool modifyStats = false;
    public Penguin.StatModifier statModifier;

    private void Awake()
    {
        instance = this;


        waves = Resources.LoadAll<WaveData>("GameData/WaveData");
        totalNumber = waves.Length;
    }

    void Start()
    {
        //StartCoroutine(SpawnEnemiesWithDelay());
    }

    private void Update()
    {
        // Input de test à retirer
        if (Input.GetKeyDown("p"))
        {
            StartCoroutine(SpawnEnemiesWithDelay());
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (GameObject obj in enemyTrackers) Destroy(obj);
        }

    }

    IEnumerator SpawnEnemiesWithDelay()
    {
        waves[waveNumber].Load();
        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Position 
            float angle = Random.Range(0f, Mathf.PI * 2);
            float radius = spawnRadius + Random.Range(-spawnRandom, spawnRandom);
            Vector3 spawnPosition = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

            // Spawn
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemyTrackers.Add(enemy);
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            enemyComponent.spawner = this;
            remainingEnemies++;
            // Delai
            float spawnDelay = Random.Range(0f, maxSpawnDelay);
            yield return new WaitForSeconds(spawnDelay);
            enemyComponent.SetStats(GameManager.instance.battleData.enemyStats);
            if (modifyStats) waves[waveNumber].statModifier.Apply(enemyComponent);
            enemyComponent.Heal(enemyComponent.baseHealth);
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
            waves[waveNumber].Finish();
            waveNumber++;
            waveNumber %= totalNumber;
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
