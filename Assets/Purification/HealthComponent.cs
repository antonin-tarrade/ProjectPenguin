using System;
using System.Collections;
using UnityEngine;

namespace Assets.Purification
{
    /// <summary>
    /// Simple health component that is damageable
    /// </summary>
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        public event Action OnHealthChange;
        public event Action OnDeath;

        [field: SerializeField] public IDamageable.Type type { get; private set; }
        [field: SerializeField] public float maxHealth { get; private set; }
        public float health { get; private set; }
        [SerializeField] private float def;

        private void Awake()
        {
            health = maxHealth;
        }

        public void DoDmg(float dmg)
        {
            health -= dmg / def;
            if (health < 0) Death();
        }

        /// <summary>
        /// Heals the object
        /// </summary>
        /// <param name="value">The amount of healing to apply</param>
        public void Heal(float value)
        {
            OnHealthChange?.Invoke();
            health = Mathf.Max(health + value, maxHealth);
        }


        private void Death()
        {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}