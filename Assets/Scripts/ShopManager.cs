using Attacks;
using System;
using TMPro;
using UnityEngine;
// using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;

public class ShopManager : MonoBehaviour
{
    // Singleton
    public static ShopManager instance;
    protected Player player;
    public static bool openable;
    private Upgrade[] attackUpgrades;
    private Upgrade[] healthUpgrades;
    private Upgrade[] passiveUpgrades;
    public TextMeshProUGUI shardText;
    public GameObject shopUI;
    public Transform shopContent;
    public GameObject itemPrefab;
    public Health healthUI;
    private UIManager uiManager;
    private GameManager gameManager;

    // getters/setters
    public Player getPlayer { get => player; set => player = value; }


    private void Awake() {
        if(instance == null)
        {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }

        attackUpgrades = new Upgrade[] {
            new SpeedUpgrade(),
            new StrengthUpgrade(),
            new MultishotUpgrade(),
        };

        healthUpgrades = new Upgrade[] {
            new HealthUpgrade(healthUI),
            new SecondChanceUpgrade(),
            new FishingUpgrade(),
        };

        passiveUpgrades = new Upgrade[] {
            new SlidingUpgrade(),
            new SlowShotUpgrade(),
        };

        player = GameObject.Find("Player").GetComponent<Player>();
        uiManager = UIManager.instance;
        gameManager = GameManager.instance;
    }

    private void Start() {
        // foreach(Upgrade upgrade in upgrades) {
        //     upgrade.player = player;
        //     GameObject item = Instantiate(itemPrefab, shopContent);
        //     upgrade.itemRef = item;
        //     foreach(Transform child in item.transform) {
        //         switch (child.gameObject.name)
        //         {
        //             case "Name":
        //                 child.gameObject.GetComponent<TextMeshProUGUI>().text = upgrade.name;
        //                 break;
        //             case "Level":
        //                 TextMeshProUGUI currentLevelText = child.gameObject.GetComponent<TextMeshProUGUI>();
        //                 currentLevelText.text = upgrade.ToString();
        //                 upgrade.currentLevelText = currentLevelText;
        //                 break;
        //             case "Price":
        //                 TextMeshProUGUI priceText = child.gameObject.GetComponent<TextMeshProUGUI>();
        //                 priceText.text = "Price : " + upgrade.price.ToString();
        //                 upgrade.priceText = priceText;
        //                 break;
        //             case "Image":
        //                 Sprite loadedImage = Resources.Load<Sprite>(upgrade.image);
        //                 child.gameObject.GetComponent<Image>().sprite = loadedImage;                  
        //                 break;
        //             default:
        //                 break;
        //         }
        //     }
        //     Button button = item.GetComponent<Button>();
        //     button.onClick.AddListener(( ) => upgrade.Buy());
        // }



        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button[] categoryButtons = new Button[] {
            root.Q<Button>("AttackButton"),
            root.Q<Button>("HealthButton"),
            root.Q<Button>("PassiveButton"),
        };

        ShopUI[] shops = new ShopUI[] {
            new ShopUI("AttackShop",Upgrade.UpgradeType.Attack,attackUpgrades,root,categoryButtons[0]),
            new ShopUI("HealthShop",Upgrade.UpgradeType.Health,healthUpgrades,root,categoryButtons[1]),
            new ShopUI("PassiveShop",Upgrade.UpgradeType.Passive,passiveUpgrades,root,categoryButtons[2]),
        };

        for (int i = 0; i < categoryButtons.Length; i++)
        {
            Button button = categoryButtons[i];
            ShopUI shop = shops[i];
            shop.shopUI.style.display = DisplayStyle.None;

            button.clickable.clicked += () => {
                Debug.Log("Click");

                foreach (var shopUI in shops)
                {
                    shopUI.shopUI.style.display = DisplayStyle.None;
                }
                shop.shopUI.style.display = DisplayStyle.Flex;
            };
        }

        shops[0].shopUI.style.display = DisplayStyle.Flex;

    }

    private void OnGUI() {
        shardText.text = "Shards : " + player.iceShards.ToString();
    }
}

// Class for the shop UI
public class ShopUI {

        public Upgrade.UpgradeType shopType;
        public string shopName;
        public Upgrade[] shopUpgrades;
        public VisualElement shopUI;
        public Dictionary<Upgrade, VisualElement> shopUpgradesUI;
        public Button upgradeButton;


        public ShopUI (string shopname, Upgrade.UpgradeType shopType, Upgrade[] shopUpgrades,VisualElement root,Button upgradeButton) {
            this.shopName = shopname;
            this.shopType = shopType;
            this.shopUpgrades = shopUpgrades;
            this.upgradeButton = upgradeButton;

            shopUI = root.Q<VisualElement>(shopName);
            shopUpgradesUI = new Dictionary<Upgrade, VisualElement>();
            for (int i = 0; i < shopUpgrades.Length; i++)
            {
                VisualElement upgradeUI = shopUI.Q<VisualElement>("Upgrade" + i);
                upgradeUI.name = shopUpgrades[i].name;
                shopUpgradesUI.Add(shopUpgrades[i],upgradeUI);
            }

            GenerateShopUI(shopUpgradesUI);
        }

        
        private void GenerateShopUI(Dictionary<Upgrade, VisualElement> shopUpgradesUI){

            foreach(var upgrade in shopUpgradesUI) {

                upgrade.Key.player = ShopManager.instance.getPlayer;
                upgrade.Key.shopUI = this;

                upgrade.Value.Q<Label>("Name").text = upgrade.Key.name;
                upgrade.Value.Q<Label>("Level").text = "Level " + upgrade.Key.currentLevel;
                upgrade.Value.Q<Label>("Price").text = "Price : " + upgrade.Key.price;

                upgrade.Value.Q<VisualElement>("Image").style.backgroundImage = Resources.Load<Texture2D>(upgrade.Key.image);
                upgrade.Value.Q<Button>("BuyButton").clickable.clicked += () => upgrade.Key.Buy();
                upgrade.Value.Q<Button>("BuyButton").text = "Buy";
            }
        }

        public void UpdateShopUI(Upgrade upgrade, int price , int currentLevel, int iceShards, bool maxLevel = false){

            if (maxLevel)
            {
                shopUpgradesUI[upgrade].Q<Label>("Price").text = "Indisponible";
                shopUpgradesUI[upgrade].Q<Label>("Level").text = "Level max atteint";
                shopUpgradesUI[upgrade].Q<Button>("BuyButton").text = "Indisponible";
                shopUpgradesUI[upgrade].Q<Button>("BuyButton").SetEnabled(false);
                
            } else {
                shopUpgradesUI[upgrade].Q<Label>("Level").text = "Level " + currentLevel;
                shopUpgradesUI[upgrade].Q<Label>("Price").text = "Price : " + price;
                shopUI.Q<Label>("Shards").text = "Shards : " + iceShards;
            }
        }

    }


// Classe abstraite dont il faut ré-implémenter la fonction d'achat (voir exemples)
public abstract class Upgrade {

    public enum UpgradeType {
        Attack,
        Health,
        Passive,
        Debuff,
    }

    public UpgradeType type { get; protected set;}
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
    public bool canBuy = false;

    public ShopUI shopUI;

    
    public virtual void Buy() {
        //Debug 
        Debug.Log("IceShards" + player.iceShards);
        Debug.Log("price : " + price);

        canBuy = player.iceShards >= price && currentLevel != levelMax ;

        if(canBuy) {
            player.iceShards -= price;
            currentLevel += 1;
            if (currentLevel != levelMax)
            {
                price *= 2;
                shopUI.UpdateShopUI(this,price,currentLevel,player.iceShards);
            } else {
                shopUI.UpdateShopUI(this,price,currentLevel,player.iceShards,true);
            }

            UpdatePlayer();
        }
    }

    public abstract void UpdatePlayer();
}

public class SecondChanceUpgrade : Upgrade {
    public SecondChanceUpgrade(){
    	name = "Second Chance";
        price = 500;
        image = "SecondChance";
        currentLevel = 0;
        levelMax = 1;
        type = UpgradeType.Health;
    }
	
    public override void UpdatePlayer() {
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
        type = UpgradeType.Attack;
    } 
    public override void UpdatePlayer(){
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
        type = UpgradeType.Health;

    }
    public override void UpdatePlayer(){
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
        type = UpgradeType.Health;
    }
    public override void UpdatePlayer() {
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
        type = UpgradeType.Passive;
    } 
    public override void UpdatePlayer() {
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
        type = UpgradeType.Attack;
    } 
    public override void UpdatePlayer() {
        player.attack.dmg += 0.5f;
    }
}

public class MultishotUpgrade : Upgrade {
    public MultishotUpgrade() {
        name = "Multishot";
        price = 5;
        image = "Multishot";
        currentLevel = 0;
	    levelMax = 3;
        type = UpgradeType.Attack;
    } 
    public override void UpdatePlayer() {
        ChangeAttack();      
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
        type = UpgradeType.Passive;
    }

    public override void UpdatePlayer()
    {
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

