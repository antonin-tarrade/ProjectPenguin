using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


// Système d'attaques et d'effets applicables aux pingouins, ainsi que les stats globales, soyez créatifs, possibilités infines !!!
namespace Attacks
{

    [CreateAssetMenu(fileName = "BattleData", menuName = "GameData/BattleData", order = 1)]
    public class BattleData : ScriptableObject
    {
        public string difficultyName;
        public Penguin.Stats playerStats;
        public Penguin.Stats enemyStats;

    }


    // Implémentation d'un système pour différentes attaques
    public interface IAttack
    {
        public float dmg { get; set; }
        public float speed { get; set; }
        public Penguin attacker { get; set; }
        public List<StatusEffect> effects { get; set; }
        public void Fire(Vector3 direction);

        public void OnHit(Penguin targetHit);

    }

   

    public class BasicAttack : IAttack
    {
        public float dmg { get; set; }

        public float speed { get; set; }

        public Penguin attacker { get; set; }

        public List<StatusEffect> effects { get; set; } = new();

        public virtual void Fire(Vector3 direction)
        {
            Vector3 offset = direction;
            GameObject projectile = GameObject.Instantiate(attacker.projectilePrefab, attacker.transform.position + offset, Quaternion.identity);
            projectile.GetComponent<Projectile>().speed = speed;
            projectile.GetComponent<Projectile>().onHit += OnHit;
            projectile.GetComponent<Projectile>().direction = direction;
            projectile.GetComponent<Projectile>().owner = attacker;
        }

        public void OnHit(Penguin targetHit)
        {
            targetHit.Hit(dmg);
            foreach (StatusEffect effect in effects)
            {
                effect?.ApplyOn(targetHit);
            }
        }
    }

    public class MultiShotAttack : BasicAttack
    {

        public float totalAngle = 30f;

        public int numberOfAttacks = 3;

        public override void Fire(Vector3 direction)
        {
            float step = totalAngle / numberOfAttacks;
            for (float angle = -totalAngle/(2); angle <= totalAngle / 2; angle += step)
            {
                Vector3 d = Helper.Rotate(direction, angle * Mathf.Deg2Rad);
                FireOne(d);
            }
        }

        private void FireOne(Vector3 direction)
        {
            Vector3 offset = direction;
            GameObject projectile = GameObject.Instantiate(attacker.projectilePrefab, attacker.transform.position + offset, Quaternion.identity);
            Debug.Log(speed);
            projectile.GetComponent<Projectile>().speed = speed;
            projectile.GetComponent<Projectile>().onHit += OnHit;
            projectile.GetComponent<Projectile>().direction = direction;
            projectile.GetComponent<Projectile>().owner = attacker;
        }
    }


    // Système d'effet sur les tirs
    public interface StatusEffect
    {
        public string name { get; }
        public float duration { get; set; }
        public void ApplyOn(Penguin target);
        public void CancelOn(Penguin target);
    }

    public class SlowStatusEffect : StatusEffect
    {
        public string name { get => "SlowEffect"; }

        private Dictionary<Penguin, float> speedRefs = new();

        public float power { get; set; }
        public float duration { get; set; }

        public void ApplyOn(Penguin target)
        {
            if (speedRefs.ContainsKey(target)) return;
            speedRefs.Add(target, target.speed);
            target.speed /= power;
            new BackgroundUpdater.Timer(duration,  () => { CancelOn(target); } );
        }

        public void CancelOn(Penguin target) 
        {
            target.speed = speedRefs[target];
            speedRefs.Remove(target);
        }




    }



}
