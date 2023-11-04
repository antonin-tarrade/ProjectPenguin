using UnityEngine;
using UnityEngine.SceneManagement;

public class IglooSystem : MonoBehaviour
{
    [SerializeField]
    private Canvas shopButton;
    private bool canOpenShop = false;
    private GameManager gameManager;

    private void Start() {
        shopButton.gameObject.SetActive(true);
        shopButton.enabled = false;
        gameManager = GameManager.instance;
    }

    // On rend le shop ouvrable quand le joueur est à proximité
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            shopButton.enabled = true;
            canOpenShop = true;            
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            shopButton.enabled = false;
            canOpenShop = false;
        }
    }


    private void Update() {
        if (!gameManager.isShopOpen && canOpenShop && !gameManager.isPaused && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.T))) {
            gameManager.isShopOpen = true;
            SceneManager.LoadScene("Shop", LoadSceneMode.Additive);
            gameManager.ShopPause();
        } else if (gameManager.isShopOpen && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.T))) {
            SceneManager.UnloadSceneAsync("Shop");
            gameManager.ShopUnpause();
            gameManager.isShopOpen = false;
        }
    }
}
