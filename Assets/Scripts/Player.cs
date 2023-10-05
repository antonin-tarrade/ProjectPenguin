using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attacks;
using UnityEngine.SceneManagement;

public class Player : Penguin
{

	private bool hasSecondChance = false;

	// Bouclier
	private bool hasBouclier = false;
	private float dureeBouclierMax = 3;
	private float dureeBouclier = 0;
	private float tempsAvantProcActInit = 10;
	private float tempsAvantProcAct = 0;
	private ProtectionStatusEffect protec;
	[SerializeField] GameObject bouclier;
	[SerializeField] BoxCollider2D colliderPlayer;

	public Vector3 spawnPoint;

	// Booleens pour empecher les mouvements diagonaux
	private bool movement_x_lock = false;
	private bool movement_y_lock = false;

	// Inputs directionnels enregistres a la derniere frame
	private Vector2 old_input = Vector2.zero;

	// Score (iceShards étant rentré dans l'igloo)
	public int iceShards = 0;
	
	public int score = 0;

	private void Start ()
	{
		InitPenguin ();
		type = Type.Player;

		GameManager.instance.playerRespawnEvent += Respawn;
	}

	public override void InitPenguin()
	{
		base.InitPenguin();
        SetStats(GameManager.instance.battleData.playerStats);
    }

    private void Update ()
	{
		UpdateDepTime();
		if (dureeBouclier <=0)
		{
			bouclier.SetActive(false);
			colliderPlayer.enabled = true;
		}
		
		if (isDead) return;
		// Arret du slide
		if (Input.GetKeyUp ("space") || Input.GetMouseButtonUp(1) || Input.anyKeyDown || movement.magnitude < 0.01)
			isSliding = false;
		// Debut du slide
		if (Input.GetKeyDown ("space") || Input.GetMouseButtonDown(1)|| Input.GetKeyDown(KeyCode.F))
		{
			isSliding = true;
			movement.x *= slideBoost;
			movement.y *= slideBoost;
			StartCoroutine (slide ());
		}

		if (!isSliding && (Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButtonDown(0)|| Input.GetKeyDown(KeyCode.R)) && !GameManager.instance.isPaused) 
		{
			Fire();
		}
		
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
		
		//Activation du bouclier
		if (hasBouclier && (Input.GetKeyDown(KeyCode.C) || Input.GetMouseButtonDown(1)))
		{
			ActiverBouclier();
		}
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

	public void AddShards(int value)
	{
		iceShards += value;
		AddScore(value);
	}
	
	public void AddScore(int value)
	{
		score += value;
	}

	public void Heal(int value)
	{
		health += value;
		health = Mathf.Min(health, baseHealth);
	}
	
	// Bouclier
	public bool GetHasBouclier()
	{
		return hasBouclier;
	}
	public void ActiverDispoBouclier()
	{
		hasBouclier = true;
		protec = new ProtectionStatusEffect() { duration = 5 };
		//Player.attack.effects.Add(new ProtectionStatusEffect() { duration = 5 });
	}
	
	private void ActiverBouclier()
	{
		
		if (tempsAvantProcAct < 0) 
		{
			tempsAvantProcAct = tempsAvantProcActInit;
			dureeBouclier = dureeBouclierMax;
			bouclier.SetActive(true);
			colliderPlayer.enabled = false;
			protec.ApplyOn(this);
			Debug.Log("Bouclier activé");
		} else {
			Debug.Log("Le bouclier n'a pas pu être activé");
		}
	}
	
	
	// Fait l'update de toutes les variables dépendants du temps pour pouvoir faire avec Time.deltaTime
	private void UpdateDepTime()
	{
		float delta = Time.deltaTime;
		tempsAvantProcAct -= delta;
		dureeBouclier -= delta;
	}
	
	public void SetSecondChance()
	{
		hasSecondChance = true;
	}

    public override void Death()
    {
		isDead = true;
		movement = new Vector2(0, 0);
		GameManager.instance.PlayerDeath();
    }

	private void Respawn()
	{	
		if (hasSecondChance)
		{
			hasSecondChance = false;
			isDead = false;
			health = baseHealth;
			transform.position = spawnPoint;
		} else {
			SceneManager.LoadScene("SampleScene");
		}
		
	}
}
