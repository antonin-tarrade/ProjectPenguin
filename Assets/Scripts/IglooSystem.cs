using UnityEngine;
using UnityEngine.SceneManagement;

public class IglooSystem : MonoBehaviour
{
    [SerializeField]
    private Canvas shopButton;
    private bool isShopOpen = false;

    private void Start() {
        shopButton.gameObject.SetActive(true);
        shopButton.enabled = false;
    }

    // On rend le shop ouvrable quand le joueur est à proximité
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


    private void Update() {
        if (!isShopOpen &&Input.GetKeyDown(KeyCode.E)) {
            isShopOpen = true;
            SceneManager.LoadScene("Shop", LoadSceneMode.Additive);
            GameManager.instance.ShopPause();
        } else if (isShopOpen && Input.GetKeyDown(KeyCode.E)) {
            SceneManager.UnloadSceneAsync("Shop");
            GameManager.instance.Unpause();
            isShopOpen = false;
        }
    }
}
