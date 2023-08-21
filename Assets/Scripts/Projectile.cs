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
		Projectile p;
		if(collision.gameObject.CompareTag("Obstacle") 
			|| collision.gameObject.CompareTag("Shards") 
			|| collision.gameObject.TryGetComponent<Projectile>(out p))
        {
            Destroy (gameObject);
			return;
        }

		// Collision avec un autre pengouin
		Penguin penguin = collision.transform.GetComponent<Penguin>();
        if (penguin != null )
		{ 
			if (!penguin.isSliding && penguin.type != owner.type)
			{
				onHit?.Invoke(penguin);
				Destroy(gameObject);
			}
		}

    }
}