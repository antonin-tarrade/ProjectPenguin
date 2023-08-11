using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Penguin
{
	// Booleens pour empecher les mouvements diagonaux
	private bool movement_x_lock = false;
	private bool movement_y_lock = false;

	// Inputs directionnels enregistrés à la dernière frame
	private Vector2 old_input = Vector2.zero;

	private void Start ()
	{
		InitPenguin ();
	}

	private void Update ()
	{
		// Arrêt du slide
		if (Input.GetKeyUp ("space") || Input.anyKeyDown || movement.magnitude < 0.01)
			isSliding = false;
		// Début du slide
		if (Input.GetKeyDown ("space"))
		{
			isSliding = true;
			movement.x *= slideBoost;
			movement.y *= slideBoost;
			StartCoroutine (slide ());
		}

		if (!isSliding && Input.GetKey (KeyCode.LeftShift))
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

	// Obtient la direction dans laquelle se déplacer en fonction des inputs
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
