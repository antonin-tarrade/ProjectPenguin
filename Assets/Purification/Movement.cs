using System.Collections;
using UnityEngine;

namespace Assets.Purification
{
    /// <summary>
    /// Components to move any object 
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour
    {

        private Rigidbody2D body;
        [SerializeField] private float speed;


        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }


        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }

        /// <summary>
        /// Modifies the speed for a certain duration
        /// </summary>
        /// <param name="speed">The modifier to apply</param>
        /// <param name="time">The duration</param>
        public void ModifySpeed(float modifier, float time)
        {
            StartCoroutine(CModifySpeed(modifier, time));
        }

        private IEnumerator CModifySpeed(float modifier, float time)
        {
            speed *= modifier;
            yield return new WaitForSeconds(time);
            speed /= modifier;
        }

        /// <summary>
        /// Sets the direction for the movement of the object
        /// </summary>
        /// <param name="direction">The direction in which to move</param>
        public void Move(Vector3 direction)
        {
            body.velocity = direction.normalized * speed;
        }
    }
}