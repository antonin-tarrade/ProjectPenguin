using System;
using System.Collections;
using UnityEngine;

namespace Assets.Purification
{
    /// <summary>
    /// Simple projectile that goes straight forward until it hits something
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        /// <summary>
        /// Collision event called upon collision with another object
        /// </summary>
        public event Action<GameObject> OnCollision;

        private float lifeLength;

        private Rigidbody2D body;
        private bool isInitiated;
        private float lifeTime;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            body.velocity = Vector2.zero;
            isInitiated = false;
            lifeTime = 0;
        }

        /// <summary>
        /// Starts the movement of the projectile for the given values
        /// </summary>
        /// <param name="direction">The direction of the projectile</param>
        /// <param name="speed">The speed of the projectile</param>
        /// <param name="lifeLength">The max duration of the projectile</param>
        public void Begin(Vector2 direction, float speed, float lifeLength)
        {
            if (isInitiated) return;
            this.lifeLength = lifeLength;
            body.velocity = direction.normalized * speed;
            isInitiated = true;
        }

        public void Update()
        {
            if (isInitiated)
            {
                lifeTime += Time.deltaTime;
                if (lifeTime > lifeLength)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollision?.Invoke(collision.gameObject);
            Destroy(gameObject);
        }
    }
}