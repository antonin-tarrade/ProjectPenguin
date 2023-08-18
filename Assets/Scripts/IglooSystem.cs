using UnityEngine;

public class IglooSystem : MonoBehaviour
{
    [SerializeField]
    private Canvas shopButton;
    private void Start() {
        shopButton.gameObject.SetActive(true);
        shopButton.enabled = false;
    }

    // On rend le shop ouvrable quand le joueur est à proximité
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            shopButton.enabled = true;     
            ShopManager.openable = true;       
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            shopButton.enabled = false;
            ShopManager.openable = false;
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
