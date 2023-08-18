using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    [SerializeField]
    private ShopManager shopManager;
    private void OnTriggerEnter2D(Collider2D other) {
        if(ShopManager.openable)
        {
            ShopManager.instance.ToggleShop();
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(shopManager.shopUI.activeSelf)
        {
            ShopManager.instance.ToggleShop();
        }
    }
}
