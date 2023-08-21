using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	// Stats
    public float speed;
    public Vector2 direction;
	public float maxLifeLength; // Duree de vie en secondes

	[HideInInspector] public Penguin owner; // Pingouin ayant tiré ce projectile
	private Rigidbody2D body;
	private float currentLifeLength = 0f;

	// Event quand le projectile touche une cible
	public delegate void OnHitPenguinEvent(Penguin penguin);
	public OnHitPenguinEvent onHit;


	private void Start ()
	{
		body = GetComponent<Rigidbody2D>();
	}

	private void Update ()
	{
		currentLifeLength += Time.deltaTime;

		if (currentLifeLength > maxLifeLength)
			Destroy (gameObject);
	}

	private void FixedUpdate ()
    {
		body.velocity = direction * speed;
	}

	private void OnCollisionEnter2D (Collision2D collision)
	{
		// Collisions entrainant la destruction (Obstacle et Shards)
		if(collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Shards"))
        {
            Destroy (gameObject);
        }

		// Collision avec un autre pengouin
		Penguin penguin = collision.transform.GetComponent<Penguin>();
		if (penguin != null && penguin != owner)
		{ 
			if (!penguin.isSliding)
			{
				// Debug.Log (collision.name + " touché par " + owner.name + " !");

				// Friendly fire Désactivé
				/*if(!(owner.gameObject.CompareTag(collision.gameObject.tag)))
				{
					penguin.health -= 1;
					Destroy (gameObject);
				}
				else{
					Destroy (gameObject);
				}
				*/

				// Friendly fire Activé
				//penguin.health -= 0.5f;
				onHit?.Invoke(penguin);
				Destroy (gameObject);
				
			}
		}
	}
}