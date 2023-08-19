using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : MonoBehaviour
{
	[Header ("Penguin")]
	// Stats
	
	public int baseHealth = 3;
	public float health;
	
	public float speed;
	public float slideBoost;
	public float slideSlowDown;
	public float fireCooldown;
	// Prefabs
	public GameObject projectilePrefab;

	// Deplacements du pingouin
	protected Vector2 movement = Vector2.zero;
	protected Vector2 facingDirection = Vector2.down;
	[HideInInspector] public bool isSliding = false;
	protected float timeAtLastFire = 0f;

	// Composants
	protected Animator animator;
	protected Rigidbody2D body;

	protected void InitPenguin ()
	{
		animator = gameObject.GetComponent<Animator> ();
		body = gameObject.GetComponent<Rigidbody2D> ();

		animator.SetFloat ("speed", 0);
		animator.SetInteger ("orientation", 0);

		// Health is a float to represent half-hearts
		health = (float)baseHealth;
	}

	protected void Move ()
	{
		body.velocity = movement;
	}

	protected IEnumerator slide ()
	{
		while (isSliding)
		{
			movement.x = slideSlowDown * movement.x;
			movement.y = slideSlowDown * movement.y;
			if (Mathf.Abs (movement.x) + Mathf.Abs (movement.y) < 0.1)
				isSliding = false;
			yield return new WaitForSeconds (0.5f);
		}
	}

	protected void Fire ()
	{
		if (isSliding)
			return;
		if (Time.time - timeAtLastFire < fireCooldown)
			return;

		timeAtLastFire = Time.time;

		Vector3 offset = facingDirection;
		GameObject projectile = Instantiate (projectilePrefab, transform.position + offset , Quaternion.identity);
		projectile.GetComponent<Projectile> ().direction = facingDirection;
		projectile.GetComponent<Projectile> ().owner = this;
	}
}
