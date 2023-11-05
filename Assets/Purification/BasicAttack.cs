using Assets.Purification.Effects;
using Attacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Purification
{
    /// <summary>
    /// Simple attack consisting in a single projectile 
    /// </summary>
    public class BasicAttack : MonoBehaviour, IAttack
    {
        [Header("Projectile parameters")]
        [Tooltip("The prefab of the projectile to use when attacking")]
        [SerializeField] private Projectile projectile;
        [SerializeField] private float atk;
        [SerializeField] protected float attackSpeed;
        [SerializeField] private float projectileLifeLength;

        protected List<IEffect<Component>> effects;

        private IDamageable.Type type;

        private void Awake()
        {
            IEffect<Component> damageEffect = (IEffect<Component>)new DamageEffect(atk);
            damageEffect.CanApply = IsSameType;
            effects = new List<IEffect<Component>>
            {
                damageEffect
            };
            if (TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                type = damageable.type;
            }
            else
            {
                type = IDamageable.Type.Other;
            }
        }

        
        public void Fire(Vector3 direction)
        {
            Projectile proj = GameObject.Instantiate(projectile, transform.position + direction.normalized, Quaternion.identity);
            proj.OnCollision += OnHit;
            proj.Begin(direction, attackSpeed, projectileLifeLength);
        }


        private void OnHit(GameObject obj)
        {
            foreach (IEffect<Component> effect in effects)
            {
                effect.ApplyOn(obj);
            }
        }

        private bool IsSameType(GameObject obj)
        {
            if (obj.TryGetComponent<IDamageable>(out IDamageable d))
            {
                if (d.type != type) return true;
                return false;
            }
            return true;
        }

    }
}