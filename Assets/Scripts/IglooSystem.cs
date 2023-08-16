using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IglooSystem : MonoBehaviour
{
    [SerializeField]
    private Canvas shopButton;

    private void Start() {
        shopButton.gameObject.SetActive(true);
        shopButton.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            shopButton.enabled = true;            
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            shopButton.enabled = false;
        }
    }
    // public GameObject player;
    // private Player playerSystem;

    // private void Awake() {
    //     //Initialisation Component
    //     playerSystem = player.GetComponent<Player>();
    // }
    // private void OnCollisionEnter2D(Collision2D collision)
	// {
	// 	if (collision.gameObject.CompareTag("Shards"))
	// 	{
	// 		Destroy(collision.gameObject);
    //         playerSystem.iceShards+=1; // Incrementation du score
	// 	}
	// }
}
