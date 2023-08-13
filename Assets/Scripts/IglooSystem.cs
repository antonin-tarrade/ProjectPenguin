using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IglooSystem : MonoBehaviour
{
    public GameObject player;
    private Player playerSystem;

    private void Awake() {
        //Initialisation Component
        playerSystem = player.GetComponent<Player>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Shards"))
		{
			Destroy(collision.gameObject);
            playerSystem.iceShards+=1; // Incrementation du score
		}
	}
}
