using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Purification
{
    /// <summary>
    /// Interface that caracterizes any type of object that can recieve damage
    /// </summary>
    public interface IDamageable
    {
        enum Type { Other, Player, Enemy }
        Type type { get; }

        /// <summary>
        /// Event called upon any modification of health
        /// </summary>
        event Action OnHealthChange;
        /// <summary>
        /// Event called upon the death of the object
        /// </summary>
        event Action OnDeath;

        /// <summary>
        /// Inflict damage to the damageable object
        /// </summary>
        /// <param name="dmg">The amount of damage to inflict</param>
        void DoDmg(float dmg);
    }
}