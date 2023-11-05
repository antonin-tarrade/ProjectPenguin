using System.Collections;
using UnityEngine;

namespace Assets.Purification
{
    [RequireComponent(typeof(Pathfinder))]
    [RequireComponent(typeof(Penguin))]
    [RequireComponent (typeof(Pathfinder))]
    public class Enemy : MonoBehaviour
    {
        [Tooltip("The prefab of the object to drop after death")]
        public GameObject drop;

        private Pathfinder pathfinder;
        private Penguin penguin;
        private HealthComponent health;

        private void Awake()
        {
            pathfinder = GetComponent<Pathfinder>();
            penguin = GetComponent<Penguin>();
            health = GetComponent<HealthComponent>();

            health.OnDeath += Drop;
        }

        private void Update()
        {
            penguin.Move(pathfinder.currentDirection);
        }



        public void Drop()
        {
            Instantiate(drop, transform.position, Quaternion.identity);
        }
    }
}