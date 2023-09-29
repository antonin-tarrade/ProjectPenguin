using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ennemies
{
    [Serializable]
    public class SpawnData
    {
        public GameObject ennemyPrefab;
        public int numberOfEnnemies;
        public PenguinStatModifier statModifier;
        public float delayBeforeNextWave = 0.1f;
        public float delayBetweenEachEnnemy = 0.1f;
    }


    [CreateAssetMenu(fileName = "WaveData", menuName = "GameData/WaveData", order = 1)]
    public class WaveData : ScriptableObject
    {
        // Peut �tre utile pour faire des sortes de paliers progressifs
        public UnityEvent onLoading;
        public UnityEvent onFinished;


        // Ennemis de la vague
        //public int numberOfEnnemies;
        //public int numberOfBoss;
        //public int numberOfSlime;
        //public GameObject ennemyPrefab;

        public List<SpawnData> spawnDatas;


        // Cocher pour changer les variables du spawner
        public bool changeSpawnParameters;
        public float spawnRadius;
        public float spawnRandom;
        public float maxSpawnDelay;

        // Cocher pour modifier les stats 

        public void Load()
        {

            EnemySpawner spawner = EnemySpawner.instance;

            spawner.waveToSpawn = spawnDatas;

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
