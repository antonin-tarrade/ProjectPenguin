using System.Collections;
using UnityEngine;

namespace Assets.Purification
{
    /// <summary>
    /// Contains all actions related to a basic penguin
    /// </summary>
    public class Penguin : Movement
    {
        private Animator animator;
        private Rigidbody2D body;

        [Tooltip("The boost given at the start of the slide")]
        [SerializeField] private float slideBoost;
        [Tooltip("How much the slide will slow down with time")]
        [SerializeField] private float slideSlowDown;
        [Tooltip("How frequently the slide will slow down")]
        [SerializeField] private float slowDownRate;
        private bool isSliding;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            body = GetComponent<Rigidbody2D>();
        }

        public void Animate()
        {
            if (body.velocity.y < 0) // Down
                animator.SetInteger("orientation", 0);
            if (body.velocity.x < 0) // Left
                animator.SetInteger("orientation", 1);
            if (body.velocity.y > 0) // Up
                animator.SetInteger("orientation", 2);
            if (body.velocity.x > 0) // Right
                animator.SetInteger("orientation", 3);
            animator.SetBool("isSliding", isSliding);
            animator.SetFloat("speed", body.velocity.magnitude);
        }

        public void StartSlide()
        {
            if (!isSliding) StartCoroutine(Slide());
        }

        public void StopSlide()
        {
            StopCoroutine(Slide());
        }

        private IEnumerator Slide()
        {
            isSliding = true;
            float minSpeed = 0.1f;
            body.velocity *= slideBoost;
            while (body.velocity.magnitude > minSpeed)
            {
                body.velocity *= slideSlowDown;
                yield return new WaitForSeconds(0.5f);
            }
            isSliding = false;
        }

    }
}