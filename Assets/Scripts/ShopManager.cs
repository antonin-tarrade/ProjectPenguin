using Attacks;
using System;
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
            new HealthUpgrade(healthUI),
            new FishingUpgrade(fishingHole),
            new SpeedUpgrade(),
            new SlidingUpgrade(),
            new StrengthUpgrade(),
            new MultishotUpgrade(),
            new SlowShotUpgrade(),
            new SecondChanceUpgrade()
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
            Button button = item.GetComponent<Button>();
            button.onClick.AddListener(( ) => upgrade.Buy());
            
            

        }
    }

    private void Update() {
        if(openable && (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.T))) {
            ToggleShop();
        }
    }

    public void ToggleShop() {
       if (shopUI.activeSelf) {
           shopUI.SetActive(false);
           gameManager.Unpause();
       }
       else {
            shopUI.SetActive(true);
            GameObject.Find("Content").transform.GetChild(0).GetComponent<Button>().Select();
            gameManager.ShopPause();
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
    LEVEL3
}

// Classe abstraite dont il faut ré-implémenter la fonction d'achat (voir exemples)
public abstract class Upgrade {
    protected string name;
    protected int price;
    protected string imagePath;
    protected LevelEnum level;
    protected LevelEnum levelMax;
    private Player player;
    public GameObject itemRef;
    private TextMeshProUGUI priceText;
    private TextMeshProUGUI levelText;
 

    public string Name { get => name; protected set => name = value; }
    public int Price { get => price; protected set => price = value; }
    public string Image { get => imagePath; protected set => imagePath = value; }
    public LevelEnum Level { get => level; protected set => level = value; }
    public LevelEnum LevelMax { get => levelMax; protected set => levelMax = value; }
    public Player Player { get => player; set => player = value; }

    public TextMeshProUGUI PriceText { get => priceText; set => priceText = value; }
    public TextMeshProUGUI LevelText { get => levelText; set => levelText = value; }



    public virtual void Buy() {
        Player.iceShards -= Price;
        
        Level += 1;
        if (Level != LevelMax)
        {
        	Price *= 2;
        	PriceText.text = "Price : " + Price.ToString();
        	LevelText.text = Level.ToString();
        } else {
        	PriceText.text = "Indisponible";
        	LevelText.text = "Level max atteint";
        }
    }
}

public class SecondChanceUpgrade : Upgrade {
    public SecondChanceUpgrade(){
    	Name = "Second Chance";
	Price = 500;
	Image = "SecondChance";
	Level = LevelEnum.LEVEL0;
	LevelMax = LevelEnum.LEVEL1;
    }
	
    public override void Buy() {
        if(Player.iceShards >= Price && Level != LevelMax) {
            base.Buy();
            Player.SetSecondChance();
        }
    }
}

public class SpeedUpgrade : Upgrade {
    public SpeedUpgrade() {
        Name = "Speed";
        Price = 1;
        Image = "Speed";
        Level = LevelEnum.LEVEL0;
	LevelMax = LevelEnum.LEVEL3;
    } 
    public override void Buy() {
        if(Player.iceShards >= Price && Level != LevelMax) {
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
	LevelMax = LevelEnum.LEVEL3;
        healthSystem = healthUI;

    }
    public override void Buy() {
        if(Player.iceShards >= Price && Level != LevelMax) {
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
	    LevelMax = LevelEnum.LEVEL3;
        this.fishingHole = fishingHole;
    }
    public override void Buy() {
        if(Player.iceShards >= Price && Level != LevelMax) {
            base.Buy();
            IcefishingHole.fishingTime *= 0.7f;
        }
    }
}


public class SlidingUpgrade : Upgrade {
    public SlidingUpgrade() {
        Name = "Sliding Distance";
        Price = 1;
        Image = "Feather";
        Level = LevelEnum.LEVEL0;
	LevelMax = LevelEnum.LEVEL3;
    } 
    public override void Buy() {
        if(Player.iceShards >= Price && Level != LevelMax) {
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
	LevelMax = LevelEnum.LEVEL3;
    } 
    public override void Buy() {
        if(Player.iceShards >= Price && Level != LevelMax) {
            base.Buy();
            Player.gameObject.GetComponent<Penguin>().attack.dmg += 0.5f;
        }
    }
}

public class MultishotUpgrade : Upgrade {
    public MultishotUpgrade() {
        Name = "Multishot";
        Price = 5;
        Image = "Multishot";
        Level = LevelEnum.LEVEL0;
	LevelMax = LevelEnum.LEVEL3;
    } 
    public override void Buy() {
        if (Player.iceShards >= Price && Level != LevelMax)
        {
            base.Buy();

            ChangeAttack();      
        }
    }

    public void ChangeAttack()
    {
        if (level == LevelEnum.LEVEL1)
        {
            IAttack newAttack = new MultiShotAttack
            {
                attacker = Player,
                dmg = Player.attack.dmg/2,
                speed = Player.attack.speed,
                effects = Player.attack.effects
            };

            Player.attack = newAttack;
        }
        else
        {
            MultiShotAttack attack = (MultiShotAttack)Player.attack;
            attack.numberOfAttacks += 2;
            attack.totalAngle += 10;
        }
    }
}

public class SlowShotUpgrade : Upgrade
{
    public SlowShotUpgrade()
    {
        Name = "SlowShot";
        Price = 1;
        Image = "Slowshot";
        Level = LevelEnum.LEVEL0;
	LevelMax = LevelEnum.LEVEL1;
    }

    public override void Buy()
    {
        if (Player.iceShards >= Price)
        {
            base.Buy();
            if (level == LevelEnum.LEVEL1)
            {
                Player.attack.effects.Add(new SlowStatusEffect() { duration = 5, power = 1.5f });
            }
            else
            {
                SlowStatusEffect effect = (SlowStatusEffect)Player.attack.effects.Find(effect => effect.name == "SlowEffect");
                effect.power += 0.5f;
                effect.duration += 1;
            }
        }
        
    }
}

// On peut penser à une autre upgrade qui réduit le cooldown des tirs ;)
