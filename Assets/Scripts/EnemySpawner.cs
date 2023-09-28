using Ennemies;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    public GameObject enemyPrefab;
    public GameObject bossPrefab;
    public GameObject slimePrefab;

    public List<GameObject> enemyTrackers;

    // Nombre d'enemi par spawn
    public int numberOfEnemies;
    public int numberOfBoss;
    public int numberOfSlime;
    public List<SpawnData> waveToSpawn;

    // Moyenne du cercle d'apparition
    public float spawnRadius;
    // Incertitude du cercle de Spawn 
    public float spawnRandom;

    // Délai entre les spawns individuel
    public float maxSpawnDelay;

    public int waveNumber = 0;
    public int totalNumber;
    public WaveData[] waves;
    public static int remainingEnemies = 0;
    public int waveDelay = 30;
    public TextMeshProUGUI timer;
    public GameObject timerGO;


    public bool modifyStats = false;
    //public Penguin.StatModifier statModifier;

    private void Awake()
    {
        instance = this;


        waves = Resources.LoadAll<WaveData>("GameData/WaveData");
        IEnumerable<WaveData> sortedWaves = waves.OrderBy(w => int.Parse(w.name)).ToArray();
        waves = sortedWaves.ToArray();
        totalNumber = waves.Length;
    }

    void Start()
    {
        StartCoroutine(SpawnEnemiesWithDelay());

        GameManager.instance.playerDeathEvent += ClearWave;
        GameManager.instance.playerRespawnEvent += StartWave;
    }

    private void Update()
    {
        /*// Input de test à retirer
        if (Input.GetKeyDown("p"))
        {
            StartCoroutine(SpawnEnemiesWithDelay());
        }
        */
        if (Input.GetKeyDown(KeyCode.R))
        {
            ClearWave();
        }

    }


    IEnumerator SpawnEnemiesWithDelay()
    {
        waves[waveNumber].Load();
        foreach (SpawnData spawnData in waveToSpawn)
        {

            //remainingEnemies = numberOfEnemies + numberOfBoss + numberOfSlime;
            remainingEnemies += spawnData.numberOfEnnemies;
            // Spawn basic ennemies
            for (int i = 0; i < spawnData.numberOfEnnemies; i++)
            {
                // Position 
                float angle = Random.Range(0f, Mathf.PI * 2);
                float radius = spawnRadius + Random.Range(-spawnRandom, spawnRandom);
                Vector3 spawnPosition = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

                // Spawn
                GameObject enemy = Instantiate(spawnData.ennemyPrefab, spawnPosition, Quaternion.identity);
                enemyTrackers.Add(enemy);
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                enemyComponent.spawner = this;
                //remainingEnemies++;
                // Delai
                // Modifier le minspawndelay, mais le min ne doit pas être trop court pour des problèmes d'opérations effectué sur l'objet instancié
                float spawnDelay = Random.Range(maxSpawnDelay / 2f, maxSpawnDelay);
                yield return new WaitForSeconds(spawnDelay);
                enemyComponent.SetStats(GameManager.instance.battleData.enemyStats);
                if (spawnData.statModifier.modifyStats) spawnData.statModifier.Apply(enemyComponent);
                enemyComponent.health = enemyComponent.baseHealth;
            }
        }
    }

    // On notifie le spawner qu'un enemi est mort, pour garder le compte des enemis encore en vie
    // et connaître l'avancement d'une vague d'enemis
    public void NotifyDeath()
    {
        remainingEnemies--;
        if (remainingEnemies == 0)
        {   
            waves[waveNumber].Finish();
            timerGO.SetActive(true);
            StartCoroutine(WaitForNextWave());
        }
    }
    
    // principalement pour les slimes pour garder le bon compte du nombre d'ennemis (pas utile probablement finalement)
    public void NotifyDeath(int numberNewEnemies)
    {
    	remainingEnemies += numberNewEnemies;
    	NotifyDeath();
    }

    private IEnumerator WaitForNextWave()
    {
        
        for (int i = waveDelay; i > 0; i--)
        {
            timer.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        timerGO.SetActive(false);
        waveNumber++;
        waveNumber %= totalNumber;
        StartCoroutine(SpawnEnemiesWithDelay());
    }

    private void ClearWave()
    {
        remainingEnemies = 0;
        foreach (GameObject obj in enemyTrackers) Destroy(obj);
        WaitForNextWave();
    }

    public void StartWave()
    {
        StartCoroutine(SpawnEnemiesWithDelay());
    }
}
