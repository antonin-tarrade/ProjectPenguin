using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PenguinStatModifier 
{
    // Modificateur de stats
    public bool modifyStats;
    public bool modifyHealth;
    public float healthModifier;
    public bool modifySpeed;
    public float speedModifier;
    public bool modifyAttackSpeed;
    public float attackSpeedModifier;
    public bool modifyDmg;
    public float dmgModifier;

    public void Apply(Penguin penguin)
    {
        if (modifyHealth) penguin.baseHealth = (int)(penguin.baseHealth * healthModifier);
        if (modifySpeed) penguin.speed *= speedModifier;
        if (modifyAttackSpeed) penguin.attack.speed *= attackSpeedModifier;
        if (modifyDmg) penguin.attack.dmg *= dmgModifier;
    }

}

[CreateAssetMenu(fileName = "StatModifier", menuName = "Scriptables/StatModifier", order = 0)]
public class StatModifierContainer : ScriptableObject
{
    public PenguinStatModifier statModifier;
}

