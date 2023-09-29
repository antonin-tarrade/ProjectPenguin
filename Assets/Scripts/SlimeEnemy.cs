using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : Enemy
{
    public int childNumber = 2;
    public GameObject smallSlimePrefab;
 

	protected override void Death() {
        Instantiate(iceShard, transform.position, Quaternion.identity);
        for (int count = 0; count < childNumber; count++)
        {
            EnemySpawner.remainingEnemies++;
            GameObject enemy = Instantiate(smallSlimePrefab, this.body.position, Quaternion.identity);
            EnemySpawner.instance.enemyTrackers.Add(enemy);
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            enemyComponent.spawner = EnemySpawner.instance;

            
            enemyComponent.SetStats(GameManager.instance.battleData.enemyStats);
            enemyComponent.health = enemyComponent.baseHealth;
            EnemySpawner.instance.enemyTrackers.Add(enemy);
            
        }
		EnemySpawner.instance.NotifyDeath();
		//Debug.Log("2 ennemies ajoutés à cause de la mort du slime");
		Destroy(gameObject);
	}
}
