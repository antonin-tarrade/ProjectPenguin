using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
   // Script de loot
    public abstract void Collected(Player player);

 
    protected void OnTriggerEnter2D (Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))
        {
            this.Collected(collision.gameObject.GetComponent<Player>());
		    Destroy(gameObject);
        }
	}

}
