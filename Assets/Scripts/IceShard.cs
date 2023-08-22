using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShards : Collectible
{
    public int value = 1;

    public override void Collected(Player player)
    {
        player.AddShards(value);
        AudioManager.instance.PlaySfxAtPoint(AudioManager.Sfx.PickUp, transform.position);
    }
}