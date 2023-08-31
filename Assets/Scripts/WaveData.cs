using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ennemies
{


    [CreateAssetMenu(fileName = "WaveData", menuName = "GameData/WaveData", order = 1)]
    public class WaveData : ScriptableObject
    {
        // Peut �tre utile pour faire des sortes de paliers progressifs
        public UnityEvent onLoading;
        public UnityEvent onFinished;


        // Ennemis de la vague
        public int numberOfEnnemies;
        public int numberOfBoss;
        public int numberOfSlime;
        public GameObject ennemyPrefab;

        // Modificateur de stats
        public bool modifyStats;
        public Penguin.StatModifier statModifier;

        // Cocher pour changer les variables du spawner
        public bool changeSpawnParameters;
        public float spawnRadius;
        public float spawnRandom;
        public float maxSpawnDelay;

        // Cocher pour modifier les stats 

        public void Load()
        {

            EnemySpawner spawner = EnemySpawner.instance;

            spawner.numberOfEnemies = numberOfEnnemies;
            spawner.numberOfBoss = numberOfBoss;
            spawner.numberOfSlime = numberOfSlime;
            if (modifyStats) EnemySpawner.instance.modifyStats = true;
            else EnemySpawner.instance.modifyStats = false;

            if (changeSpawnParameters)
            {
                
                spawner.spawnRadius = spawnRadius;
                spawner.spawnRandom = spawnRandom;
                spawner.maxSpawnDelay = maxSpawnDelay;
            }

            onLoading?.Invoke();
        }

       

        public void Finish()
        {
            onFinished?.Invoke();
        }
    }


}
