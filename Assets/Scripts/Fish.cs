using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : Collectible {
    public int HealingAmount = 5;

    public override void Collected(Player player)
    {
        player.Heal(HealingAmount);
    }
}