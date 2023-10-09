using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Penguin
{
	[Header ("Enemy")]

	// Objet/Component
	private Player player;
	public GameObject iceShard; // Prefab Loot

	// Variables pathfinding
	public bool usePathfinder;
	Pathfinder pathfinder;
	protected Transform target;
	protected Vector2 distanceToTarget;
	protected bool isAvoiding=false;
	protected Vector2 avoidDirection; 
	protected Vector3 obstaclePosition;
	public float movementTreshold; // Distance avec le joueur qui fera se deplacer le pingouin
	public float shootingDistance; // Distance avec le joueur a laquelle le pingouin tire
	public EnemySpawner spawner;
	public float avoidance; // Puissance 

	private void Awake() {
		//Initialisation Component
		player = GameObject.FindObjectOfType<Player>();
		pathfinder = GetComponent<Pathfinder>();
		target = player.transform;
        InitPenguin();
        type = Type.Ennemy;
    }

	private void Start ()
	{
		
	}

	private void Update ()
	{
		// Déplacement et tir
		distanceToTarget.x = transform.position.x - target.position.x;
		distanceToTarget.y = transform.position.y - target.position.y;
		bool xAlignedToTarget = false;
		bool yAlignedToTarget = false;
		if (usePathfinder)
		{
			xAlignedToTarget = Mathf.Abs(distanceToTarget.x) < pathfinder.minAxisDistance;
            xAlignedToTarget = Mathf.Abs(distanceToTarget.x) < pathfinder.minAxisDistance;
        }
		else
		{
            xAlignedToTarget = Mathf.Abs(distanceToTarget.x) < 0.35f;
            yAlignedToTarget = Mathf.Abs(distanceToTarget.y) < 0.35f;
        }
		bool canShootX = Mathf.Abs (facingDirection.x) < 0.01f && xAlignedToTarget && Mathf.Abs (distanceToTarget.x) < shootingDistance;
		bool canShootY = Mathf.Abs (facingDirection.y) < 0.01f && yAlignedToTarget && Mathf.Abs (distanceToTarget.y) < shootingDistance;
		if (canShootX || canShootY){
			// Tir
			if (!usePathfinder) Fire();
			else Fire(target.transform.position);
		}
		else{
			// Calcul du déplacement du personnage (modifie movement)
			if (!usePathfinder) CaclMovement(xAlignedToTarget, yAlignedToTarget);
			else movement = (pathfinder.currentDirection.normalized) * speed;
		}
		
		//// Mort 
		//if (health <= 0){
		//	Death();
		//}

		// Orientation
		FaceTowards ();

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

	protected void CaclMovement(bool xAlignedToTarget, bool yAlignedToTarget)
	{
		if(isAvoiding){
				// Avoid Obstacle
				movement = avoidDirection;

				float obstacleDistance = Vector2.Distance(transform.position, obstaclePosition);
				if (obstacleDistance > avoidance)
				{
					isAvoiding = false;
				}
			}
			else{

				// Deplacement vers le joueur (basique ...)	
				if ((!yAlignedToTarget && Mathf.Abs (distanceToTarget.x) > 0.35f) || (yAlignedToTarget && Mathf.Abs (distanceToTarget.x) > movementTreshold))
					movement = -Vector2.right * Mathf.Sign (distanceToTarget.x);    // Deplacement selon x
				else if ((!xAlignedToTarget && Mathf.Abs (distanceToTarget.y) > 0.35f) || (xAlignedToTarget && Mathf.Abs (distanceToTarget.y) > movementTreshold))
					movement = -Vector2.up * Mathf.Sign (distanceToTarget.y);       // Deplacement selon y
				else
				{
					movement = Vector2.zero;
				}
			}
			
			movement *= speed;
	}

	private void FixedUpdate ()
	{
		Move ();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Obstacle"))
		{
			isAvoiding = true;
			obstaclePosition = collision.transform.position;
			avoidDirection = AvoidDirection(obstaclePosition);
		}
	}

	protected Vector2 AvoidDirection(Vector3 obstaclePosition)
	{
		Vector2 avoidDirection = (transform.position - obstaclePosition).normalized;
		Vector2 perpendicular = new Vector2(-avoidDirection.y, avoidDirection.x);
		Vector2 finalAvoidanceDirection = (avoidDirection + perpendicular).normalized;

		return finalAvoidanceDirection;
	}

	// Fait face dans la direction de la marche ou dans celle du joueur
	protected void FaceTowards ()
	{
		if (movement != Vector2.zero)
		{
			facingDirection = movement.normalized;
		} else
		{
			if (Mathf.Abs (distanceToTarget.x) > Mathf.Abs (distanceToTarget.y))
				facingDirection = (distanceToTarget.x > 0) ? Vector2.left : Vector2.right;
			else
				facingDirection = (distanceToTarget.y > 0) ? Vector2.down : Vector2.up;
		}
	}

	public override void Death() {
        Instantiate(iceShard, transform.position, Quaternion.identity);
		EnemySpawner.instance.NotifyDeath();
		player.AddScore(points);
		Destroy(gameObject);
	}
}
