using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    public Player player;
    public GameManager gameManager;
    public static bool openable {get; set;}
    public Health healthUI;
    public IcefishingHole fishingHole;

    private Upgrade[] upgrades;
    public TextMeshProUGUI shardText;
    public GameObject shopUI;
    public Transform shopContent;
    public GameObject itemPrefab;


    private void Awake() {
        if(instance == null)
        {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }

        upgrades = new Upgrade[] {
            new FishingUpgrade(fishingHole),
            new HealthUpgrade(healthUI),
            new SpeedUpgrade(),
            new SlidingUpgrade(),
            new StrengthUpgrade(),
            new MultishotUpgrade()
        };
    }

    private void Start() {
        foreach(Upgrade upgrade in upgrades) {
            upgrade.Player = player;
            GameObject item = Instantiate(itemPrefab, shopContent);
            upgrade.itemRef = item;
            foreach(Transform child in item.transform) {
                switch (child.gameObject.name)
                {
                    case "Name":
                        child.gameObject.GetComponent<TextMeshProUGUI>().text = upgrade.Name;
                        break;
                    case "Level":
                        TextMeshProUGUI levelText = child.gameObject.GetComponent<TextMeshProUGUI>();
                        levelText.text = upgrade.Level.ToString();
                        upgrade.LevelText = levelText;
                        break;
                    case "Price":
                        TextMeshProUGUI priceText = child.gameObject.GetComponent<TextMeshProUGUI>();
                        priceText.text = "Price : " + upgrade.Price.ToString();
                        upgrade.PriceText = priceText;
                        break;
                    case "Image":
                        Sprite loadedImage = Resources.Load<Sprite>(upgrade.Image);
                        child.gameObject.GetComponent<Image>().sprite = loadedImage;                  
                        break;
                    default:
                        break;
                }
            }

            item.GetComponent<Button>().onClick.AddListener(() => upgrade.Buy());
            
        }
    }

    private void Update() {
        if(openable && Input.GetKeyUp(KeyCode.E)) {
            ToggleShop();
        }
    }

    public void ToggleShop() {
        shopUI.SetActive(!shopUI.activeSelf);
        if(shopUI.activeSelf) {
            gameManager.Pause();
        } else {
            gameManager.Unpause();
        }
    }

    private void OnGUI() {
        shardText.text = "Shards : " + player.iceShards.ToString();
    }
}
// Enum pour les niveaux des powerup
public enum LevelEnum
{
    LEVEL0,
    LEVEL1,
    LEVEL2,
    LEVEL3,
}

// Classe abstraite dont il faut ré-implémenter la fonction d'achat (voir exemples)
public abstract class Upgrade {
    protected string name;
    protected int price;
    protected string imagePath;
    protected LevelEnum level;
    private Player player;
    public GameObject itemRef;
    private TextMeshProUGUI priceText;
    private TextMeshProUGUI levelText;
 

    public string Name { get => name; protected set => name = value; }
    public int Price { get => price; protected set => price = value; }
    public string Image { get => imagePath; protected set => imagePath = value; }
    public LevelEnum Level { get => level; protected set => level = value; }
    public Player Player { get => player; set => player = value; }

    public TextMeshProUGUI PriceText { get => priceText; set => priceText = value; }
    public TextMeshProUGUI LevelText { get => levelText; set => levelText = value; }



    public virtual void Buy() {
        Player.iceShards -= Price;
        Price *= 2;
        Level += 1;
        PriceText.text = "Price : " + Price.ToString();
        LevelText.text = Level.ToString();
    }
}

public class SpeedUpgrade : Upgrade {
    public SpeedUpgrade() {
        Name = "Speed";
        Price = 1;
        Image = "Speed";
        Level = LevelEnum.LEVEL0;
    } 
    public override void Buy() {
        if(Player.iceShards >= Price && Level != LevelEnum.LEVEL3) {
            base.Buy();
            Player.speed *= 1.2f;
        }
    }
}
public class HealthUpgrade : Upgrade {
    private Health healthSystem;

    public HealthUpgrade(Health healthUI) {
        Name = "Health";
        Price = 5;
        Image = "Heart";
        Level = LevelEnum.LEVEL0;
        healthSystem = healthUI;

    }
    public override void Buy() {
        if(Player.iceShards >= Price && Level != LevelEnum.LEVEL3) {
            base.Buy();
            Player.baseHealth += 1;
            healthSystem.InitHealthUI(Player.baseHealth);
        }
    }
}

public class FishingUpgrade : Upgrade {
    private IcefishingHole fishingHole;

    public FishingUpgrade(IcefishingHole fishingHole) {
        Name = "Fishing";
        Price = 3;
        Image = "FishingRod";
        Level = LevelEnum.LEVEL0;
        this.fishingHole = fishingHole;
    }
    public override void Buy() {
        if(Player.iceShards >= Price && Level != LevelEnum.LEVEL3) {
            base.Buy();
            fishingHole.fishingTime *= 0.7f;
        }
    }
}


public class SlidingUpgrade : Upgrade {
    public SlidingUpgrade() {
        Name = "Sliding Distance";
        Price = 1;
        Image = "Feather";
        Level = LevelEnum.LEVEL0;
    } 
    public override void Buy() {
        if(Player.iceShards >= Price && Level != LevelEnum.LEVEL3) {
            base.Buy();
            Player.slideBoost *= 1.2f;
            Player.slideSlowDown *= 0.95f;
        }
    }
}

public class StrengthUpgrade : Upgrade {
    public StrengthUpgrade() {
        Name = "Strength";
        Price = 1;
        Image = "Strength";
        Level = LevelEnum.LEVEL0;
    } 
    public override void Buy() {
        if(Player.iceShards >= Price && Level != LevelEnum.LEVEL3) {
            base.Buy();
            Debug.Log("Not Implemented yet, you just lost your shards :pepeLoser:");
        }
    }
}

public class MultishotUpgrade : Upgrade {
    public MultishotUpgrade() {
        Name = "Multishot";
        Price = 1;
        Image = "Multishot";
        Level = LevelEnum.LEVEL0;
    } 
    public override void Buy() {
        if(Player.iceShards >= Price && Level != LevelEnum.LEVEL3) {
            base.Buy();
            Debug.Log("Not Implemented yet, you just lost your shards :pepeLoser:");
        }
    }
}

// On peut penser à une autre upgrade qui réduit le cooldown des tirs ;)