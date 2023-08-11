using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : Penguin
{
	[Header ("Ennemy")]
	public float movementTreshold; // Distance avec le joueur qui fera se déplacer le pingouin
	public float shootingDistance; // Distance avec le joueur à laquelle le pingouin tire

	private Transform player;
	private Vector2 distanceToPlayer;

	private void Start ()
	{
		InitPenguin ();
		player = GameObject.FindWithTag ("Player").transform;
	}

	private void Update ()
	{
		distanceToPlayer.x = transform.position.x - player.position.x;
		distanceToPlayer.y = transform.position.y - player.position.y;

		// Déplacement (basique ...)
		bool xAlignedToPlayer = Mathf.Abs (distanceToPlayer.x) < 0.35f;
		bool yAlignedToPlayer = Mathf.Abs (distanceToPlayer.y) < 0.35f;

		if ((!yAlignedToPlayer && Mathf.Abs (distanceToPlayer.x) > 0.35f) || (yAlignedToPlayer && Mathf.Abs (distanceToPlayer.x) > movementTreshold))
			movement = -Vector2.right * Mathf.Sign (distanceToPlayer.x);    // Déplacement selon x
		else if ((!xAlignedToPlayer && Mathf.Abs (distanceToPlayer.y) > 0.35f) || (xAlignedToPlayer && Mathf.Abs (distanceToPlayer.y) > movementTreshold))
			movement = -Vector2.up * Mathf.Sign (distanceToPlayer.y);       // Déplacement selon y
		else
		{
			movement = Vector2.zero;
		}

		movement *= speed;

		// Orientation
		FaceTowards ();

		// Tir
		bool canShootX = Mathf.Abs (facingDirection.x) < 0.01f && xAlignedToPlayer && Mathf.Abs (distanceToPlayer.x) < shootingDistance;
		bool canShootY = Mathf.Abs (facingDirection.y) < 0.01f && yAlignedToPlayer && Mathf.Abs (distanceToPlayer.y) < shootingDistance;
		if (canShootX || canShootY)
			Fire ();

		// Variables d'animation
		if (facingDirection.y < 0) // Down
			animator.SetInteger ("orientation", 0);
		if (facingDirection.x < 0) // Left
			animator.SetInteger ("orientation", 1);
		if (facingDirection.y > 0) // Up
			animator.SetInteger ("orientation", 2);
		if (facingDirection.x > 0) // Right
			animator.SetInteger ("orientation", 3);
		animator.SetBool ("isSliding", isSliding);
		animator.SetFloat ("speed", movement.magnitude);
	}

	private void FixedUpdate ()
	{
		Move ();
	}

	// Fait face dans la direction de la marche ou dans celle du joueur
	private void FaceTowards ()
	{
		if (movement != Vector2.zero)
		{
			facingDirection = movement.normalized;
		} else
		{
			if (Mathf.Abs (distanceToPlayer.x) > Mathf.Abs (distanceToPlayer.y))
				facingDirection = (distanceToPlayer.x > 0) ? Vector2.left : Vector2.right;
			else
				facingDirection = (distanceToPlayer.y > 0) ? Vector2.down : Vector2.up;
		}
	}
}