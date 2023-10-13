using Attacks;
using System;
using TMPro;
using UnityEngine;
// using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Unity.VisualScripting;

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
    private Health healthUI;
    private UIManager uiManager;
    private GameManager gameManager;

    public ShopUI[] shops;
    public Button[] categoryButtons;

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

        player = GameObject.Find("Player").GetComponent<Player>();
        uiManager = UIManager.instance;
        gameManager = GameManager.instance;
        healthUI = GameObject.Find("UIHealth").GetComponent<Health>();
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

        Upgrade[] allUpgrades = player.upgradesList;

        attackUpgrades = allUpgrades.Where(upgrade => upgrade.type == Upgrade.UpgradeType.Attack).ToArray();
        healthUpgrades = allUpgrades.Where(upgrade => upgrade.type == Upgrade.UpgradeType.Health).ToArray();
        passiveUpgrades = allUpgrades.Where(upgrade => upgrade.type == Upgrade.UpgradeType.Passive).ToArray();

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        categoryButtons = new Button[] {
            root.Q<Button>("AttackButton"),
            root.Q<Button>("HealthButton"),
            root.Q<Button>("PassiveButton"),
        };

        shops = new ShopUI[] {
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
                upgrade.Value.Q<Label>("Price").text = "Price : " + upgrade.Key.prices[upgrade.Key.currentLevel];

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
            }
        }

    }


// Classe abstraite dont il faut ré-implémenter la fonction d'achat (voir exemples)
public abstract class Upgrade {

    // Enum
    public enum UpgradeType {
        Attack,
        Health,
        Passive,
        Debuff,
    }

    // Fields
    public UpgradeType type { get; protected set;}
    public string name { get; protected set;}
    public int levelMax { get; protected set;}
    public int currentLevel;
    public string image { get; set;}
    public bool canBuy = false;

    // Stats
    public int[] prices;

    // References
    public Player player { get; set;}
    public ShopUI shopUI;


    // Effects
    public abstract void UpdatePlayer();

    public virtual void Buy() {

        int currentPrice = prices[currentLevel];
        canBuy = player.iceShards >= currentPrice && currentLevel != levelMax ;

        if(canBuy) {

            player.iceShards -= currentPrice;
            currentLevel ++;
            currentPrice = prices[currentLevel];
            UpdatePlayer();

            if (currentLevel != levelMax)
            {
                shopUI.UpdateShopUI(this,currentPrice,currentLevel,player.iceShards);
            } else {
                shopUI.UpdateShopUI(this,currentPrice,currentLevel,player.iceShards,true);
            }
        }
    }

}

public class SecondChanceUpgrade : Upgrade {
    public SecondChanceUpgrade(){
    	name = "Second Chance";
        image = "SecondChance";
        currentLevel = 0;
        levelMax = 1;
        type = UpgradeType.Health;
        prices = new int[] { 500 };
    }
	
    public override void UpdatePlayer() {
        player.SetSecondChance();
    }
}

public class SpeedUpgrade : Upgrade {
    public SpeedUpgrade() {
        name = "Speed";
        image = "Speed";
	    levelMax = 4;
        currentLevel = 0;
        type = UpgradeType.Attack;
        prices = new int[] { 1, 2, 5, 10 };
    } 

    public override void UpdatePlayer() {
        player.speed += 0.5f;
    }

}
public class HealthUpgrade : Upgrade {
    private Health UI;

    public HealthUpgrade(Health healthUI) {
        name = "Health";
        image = "Heart";
	    levelMax = 4;
        currentLevel = 0;
        UI = healthUI;
        type = UpgradeType.Health;
        prices = new int[] { 1, 2, 5, 10 };
    }

    public override void UpdatePlayer() {
        player.baseHealth ++;
        UI.InitHealthUI(player.baseHealth);
        UI.UpdateHealthUI(player.baseHealth, player.health);
    }
}

public class FishingUpgrade : Upgrade {

    public FishingUpgrade() {
        name = "Fishing";
        image = "FishingRod";
	    levelMax = 4;
        currentLevel = 0;
        type = UpgradeType.Health;
        prices = new int[] { 1, 2, 5, 10 };
    }

    public override void UpdatePlayer() {
        player.fishingTime -= 0.5f;
    }
}

public class SlidingUpgrade : Upgrade {
    public SlidingUpgrade() {
        name = "Sliding Distance";
        image = "Feather";
	    levelMax = 3;
        currentLevel = 0;
        type = UpgradeType.Passive;
        prices = new int[] { 1, 5, 10 };
    } 

    public override void UpdatePlayer() {
        player.slideBoost += 0.5f;
    }
}


public class StrengthUpgrade : Upgrade {
    public StrengthUpgrade() {
        name = "Strength";
        image = "Strength";
        currentLevel = 0;
	    levelMax = 5;
        type = UpgradeType.Attack;
        prices = new int[] { 1, 2, 5, 10, 20 };
    } 

    public override void UpdatePlayer() {
        player.attack.dmg += 1;
    }
}

public class MultishotUpgrade : Upgrade {
    public MultishotUpgrade() {
        name = "Multishot";
        image = "Multishot";
        currentLevel = 0;
	    levelMax = 3;
        type = UpgradeType.Attack;
        prices = new int[] { 5, 15, 30 };
    }

    public override void UpdatePlayer()
    {
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
        image = "Slowshot";
        currentLevel = 0;
	    levelMax = 1;
        type = UpgradeType.Passive;
        prices = new int[] { 5 };
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

