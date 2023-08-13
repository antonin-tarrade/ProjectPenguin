using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    // Script de loot des Iceshards quand il fallait juste marcher dessus pour les recolter
    private void OnTriggerEnter2D (Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().iceShards += 1;
		    Destroy(gameObject);
        }
	}

}
