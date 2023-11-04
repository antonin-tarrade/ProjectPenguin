using Attacks;

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



    // Effects
    public abstract void UpdatePlayer();

    public virtual void Buy() {

        int currentPrice = prices[currentLevel];
        canBuy = Player.iceShards >= currentPrice && currentLevel != levelMax ;

        if(canBuy) {

            Player.iceShards -= currentPrice;
            currentLevel ++;
            UpdatePlayer();

            if (currentLevel != levelMax)
            {
                currentPrice = prices[currentLevel];
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
        type = UpgradeType.Passive;
        prices = new int[] { 3, 5, 20, 30 };
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
        prices = new int[] { 5, 10, 20, 30 };
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
        prices = new int[] { 3, 5, 20 };
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
	    levelMax = 3;
        type = UpgradeType.Attack;
        prices = new int[] { 5, 10, 30};
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
	    levelMax = 2;
        type = UpgradeType.Attack;
        prices = new int[] { 10, 30};
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
	    levelMax = 3;
        type = UpgradeType.Passive;
        prices = new int[] { 10, 20, 30};
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

