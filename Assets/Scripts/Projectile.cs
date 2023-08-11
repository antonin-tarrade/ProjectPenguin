using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	// Stats
    public float speed;
    public Vector2 direction;
	public float maxLifeLength; // Durée de vie en secondes

	[HideInInspector] public Penguin owner; // Pingouin ayant tiré ce projectile
	private Rigidbody2D body;
	private float currentLifeLength = 0f;


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

	private void OnTriggerEnter2D (Collider2D collision)
	{
		Penguin penguin = collision.transform.GetComponent<Penguin> ();
		if (penguin != null && penguin != owner)
		{
			if (!penguin.isSliding)
			{
				Debug.Log (collision.name + " touché par " + owner.name + " !");
				Destroy (gameObject);
			}
		}
	}
}