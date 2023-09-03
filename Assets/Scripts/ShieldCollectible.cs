using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollectible : Collectible
{
    public override void Collected(Player player)
    {
        AudioManager.instance.PlaySfxAtPoint(AudioManager.Sfx.PickUp, transform.position);
        player.ActiverDispoBouclier();
    }
}
