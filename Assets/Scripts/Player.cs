using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Penguin
{
	// Booleens pour empecher les mouvements diagonaux
	private bool movement_x_lock = false;
	private bool movement_y_lock = false;

	// Inputs directionnels enregistres a la derniere frame
	private Vector2 old_input = Vector2.zero;

	// Score (iceShards étant rentré dans l'igloo)
	public int iceShards = 0;

	private void Start ()
	{
		InitPenguin ();
	}

	private void Update ()
	{
		// Arret du slide
		if (Input.GetKeyUp ("space") || Input.GetMouseButtonUp(1) || Input.anyKeyDown || movement.magnitude < 0.01)
			isSliding = false;
		// Debut du slide
		if (Input.GetKeyDown ("space") || Input.GetMouseButtonDown(1))
		{
			isSliding = true;
			movement.x *= slideBoost;
			movement.y *= slideBoost;
			StartCoroutine (slide ());
		}

		if (!isSliding && (Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButtonDown(0)) ) 
			Fire ();

		// Mouvement
		Vector2 movementDirection = getDirectionFromInput ();
		if (movementDirection != Vector2.zero)
			facingDirection = movementDirection;

		if (!isSliding)
		{
			movement.x = speed * movementDirection.x;
			movement.y = speed * movementDirection.y;
		}

		// Variables d'animation
		if (movementDirection.y < 0) // Down
			animator.SetInteger ("orientation", 0);
		if (movementDirection.x < 0) // Left
			animator.SetInteger ("orientation", 1);
		if (movementDirection.y > 0) // Up
			animator.SetInteger ("orientation", 2);
		if (movementDirection.x > 0) // Right
			animator.SetInteger ("orientation", 3);
		animator.SetBool ("isSliding", isSliding);
		animator.SetFloat ("speed", movement.magnitude);
	}

	private void FixedUpdate ()
	{
		Move ();
	}

	// Obtient la direction dans laquelle se deplacer en fonction des inputs
	private Vector2 getDirectionFromInput ()
    {
		float input_x = Input.GetAxisRaw ("Horizontal");
        float input_y = Input.GetAxisRaw ("Vertical");

        if (!isSliding)
        {
            if (Mathf.Abs (input_x) < 0.01 || old_input.y != input_y)
            {
                movement_x_lock = true;
                movement_y_lock = false;
            }
            if (Mathf.Abs (input_y) < 0.01 || old_input.x != input_x)
            {
                movement_y_lock = true;
                movement_x_lock = false;
            }
		}

		old_input.x = input_x;
        old_input.y = input_y;

        float direction_x = movement_x_lock ? 0 : input_x;
        float direction_y = movement_y_lock ? 0 : input_y;

        return new Vector2 (direction_x, direction_y);
	}
}
