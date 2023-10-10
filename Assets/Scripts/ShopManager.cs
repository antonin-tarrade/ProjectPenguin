using Attacks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    // Singleton
    public static ShopManager instance;
    private Player player;
    public static bool openable;
    private Upgrade[] upgrades;
    public TextMeshProUGUI shardText;
    public GameObject shopUI;
    public Transform shopContent;
    public GameObject itemPrefab;
    public Health healthUI;
    private UIManager uiManager;
    private GameManager gameManager;

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
            new FishingUpgrade(),
            new SpeedUpgrade(),
            new SlidingUpgrade(),
            new StrengthUpgrade(),
            new MultishotUpgrade(),
            new SlowShotUpgrade(),
            new SecondChanceUpgrade()
        };

        player = GameObject.Find("Player").GetComponent<Player>();
        uiManager = UIManager.instance;
        gameManager = GameManager.instance;
    }

    private void Start() {
        foreach(Upgrade upgrade in upgrades) {
            upgrade.player = player;
            GameObject item = Instantiate(itemPrefab, shopContent);
            upgrade.itemRef = item;
            foreach(Transform child in item.transform) {
                switch (child.gameObject.name)
                {
                    case "Name":
                        child.gameObject.GetComponent<TextMeshProUGUI>().text = upgrade.name;
                        break;
                    case "Level":
                        TextMeshProUGUI currentLevelText = child.gameObject.GetComponent<TextMeshProUGUI>();
                        currentLevelText.text = upgrade.ToString();
                        upgrade.currentLevelText = currentLevelText;
                        break;
                    case "Price":
                        TextMeshProUGUI priceText = child.gameObject.GetComponent<TextMeshProUGUI>();
                        priceText.text = "Price : " + upgrade.price.ToString();
                        upgrade.priceText = priceText;
                        break;
                    case "Image":
                        Sprite loadedImage = Resources.Load<Sprite>(upgrade.image);
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

    private void OnGUI() {
        shardText.text = "Shards : " + player.iceShards.ToString();
    }
}

// Classe abstraite dont il faut ré-implémenter la fonction d'achat (voir exemples)
public abstract class Upgrade {
    public string name { get; protected set;}
    public int price { get; protected set;}
    public string imagePath { get; protected set;}
    public int currentLevel { get; protected set;}
    public int levelMax { get; protected set;}
    public Player player { get; set;}
    public GameObject itemRef { get; set;}


    public TextMeshProUGUI priceText { get; set;}
    public TextMeshProUGUI currentLevelText { get; set;}
    public String image { get; set;}

    
    public virtual void Buy() {
        
        if(player.iceShards >= price && currentLevel != levelMax) {

            player.iceShards -= price;
            currentLevel += 1;
            if (currentLevel != levelMax)
            {
                price *= 2;
                priceText.text = "Price : " + price;
                currentLevelText.text = "Level " + currentLevel;
            } else {
                priceText.text = "Indisponible";
                currentLevelText.text = "Level max atteint";
            }
        }
    }
}

public class SecondChanceUpgrade : Upgrade {
    public SecondChanceUpgrade(){
    	name = "Second Chance";
        price = 500;
        image = "SecondChance";
        currentLevel = 0;
        levelMax = 1;
    }
	
    public override void Buy() {
            base.Buy();
            player.SetSecondChance();
    }
}

public class SpeedUpgrade : Upgrade {
    public SpeedUpgrade() {
        name = "Speed";
        price = 1;
        image = "Speed";
        currentLevel = 0;
	    levelMax = 3;
    } 
    public override void Buy() {
            base.Buy();
            player.speed *= 1.2f;
    }
}
public class HealthUpgrade : Upgrade {
    private Health healthUI;

    public HealthUpgrade(Health healthUI) {
        name = "Health";
        price = 5;
        image = "Heart";
        currentLevel = 0;
	    levelMax = 3;
        this.healthUI = healthUI;

    }
    public override void Buy() {
        base.Buy();
        player.baseHealth += 1;
        healthUI.InitHealthUI(player.baseHealth);
    }
}

public class FishingUpgrade : Upgrade {
    private IcefishingHole fishingHole;

    public FishingUpgrade() {
        name = "Fishing";
        price = 3;
        image = "FishingRod";
        currentLevel = 0;
	    levelMax = 3;
    }
    public override void Buy() {
        base.Buy();
        player.fishingTime *= 0.7f;
    
    }
}

public class SlidingUpgrade : Upgrade {
    public SlidingUpgrade() {
        name = "Sliding Distance";
        price = 1;
        image = "Feather";
        currentLevel = 0;
	    levelMax = 3;
    } 
    public override void Buy() {
        base.Buy();
        player.slideBoost *= 1.2f;
        player.slideSlowDown *= 0.95f;
    }
}


public class StrengthUpgrade : Upgrade {
    public StrengthUpgrade() {
        name = "Strength";
        price = 1;
        image = "Strength";
        currentLevel = 0;
	    levelMax = 3;
    } 
    public override void Buy() {
        base.Buy();
        player.gameObject.GetComponent<Penguin>().attack.dmg += 0.5f;
    }
}

public class MultishotUpgrade : Upgrade {
    public MultishotUpgrade() {
        name = "Multishot";
        price = 5;
        image = "Multishot";
        currentLevel = 0;
	    levelMax = 3;
    } 
    public override void Buy() {
        if (player.iceShards >= price && currentLevel !=  levelMax)
        {
            base.Buy();

            ChangeAttack();      
        }


    }

    public void ChangeAttack()
    {
        if (currentLevel == 1)
        {
            IAttack newAttack = new MultiShotAttack
            {
                attacker = player,
                dmg = player.attack.dmg/2,
                speed = player.attack.speed,
                effects = player.attack.effects
            };

            player.attack = newAttack;
        }
        else
        {
            MultiShotAttack attack = (MultiShotAttack)player.attack;
            attack.numberOfAttacks += 2;
            attack.totalAngle += 10;
        }
    }
}

public class SlowShotUpgrade : Upgrade
{
    public SlowShotUpgrade()
    {
        name = "SlowShot";
        price = 1;
        image = "Slowshot";
        currentLevel = 0;
	    levelMax = 1;
    }

    public override void Buy()
    {
        if (player.iceShards >= price)
        {
            base.Buy();
            if (currentLevel == 1)
            {
                player.attack.effects.Add(new SlowStatusEffect() { duration = 5, power = 1.5f });
            }
            else
            {
                SlowStatusEffect effect = (SlowStatusEffect) player.attack.effects.Find(effect => effect.name == "SlowEffect");
                effect.power += 0.5f;
                effect.duration += 1;
            }
        }
        
    }
}

// On peut penser à une autre upgrade qui réduit le cooldown des tirs ;)
