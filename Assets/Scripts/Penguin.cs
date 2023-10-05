using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Attacks;
using System;

public class Penguin : MonoBehaviour
{
	[Serializable]
    public class Stats
    {
        public int baseHealth;
        public float speed;
		public float attackSpeed;
		public float dmg;
		public float def;
		public int points;

		public Stats(int baseHealth, float speed, float attackSpeed, float dmg, float def, int points)
		{
			this.baseHealth = baseHealth;
			this.speed = speed;
			this.attackSpeed = attackSpeed;
			this.dmg = dmg;
			this.def = def;
			this.points = points;
		}
    }

	public void SetStats( Stats stats )
	{
		baseHealth = stats.baseHealth;
		speed = stats.speed;
		attack.dmg = stats.dmg;
		attack.speed = stats.attackSpeed;
		def = stats.def;
		points = stats.points; 
	}

    [Header ("Penguin")]
	// Stats


	public int baseHealth = 3;
	public float health;
	
	public int points;
	public float def;
	public float speed;
	public float slideBoost;
	public float slideSlowDown;
	public float fireCooldown;
	protected bool isDead = false;

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

	// Competences
	public List<Upgrade> upgrades = new();
	public IAttack attack;

	// Pour ne pas que les ennemis se tuent entre eux
	public enum Type { Player, Ennemy }
	public Type type { get; protected set; }

    public virtual void InitPenguin ()
	{
		animator = gameObject.GetComponent<Animator> ();
		body = gameObject.GetComponent<Rigidbody2D> ();

		animator.SetFloat ("speed", 0);
		animator.SetInteger ("orientation", 0);

		// Health is a float to represent half-hearts
		health = (float)baseHealth;

        attack = new BasicAttack
        {
            dmg = 0.5f,
            attacker = this
        };
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

	protected virtual void Fire ()
	{
		if (isSliding)
			return;
		if (Time.time - timeAtLastFire < fireCooldown)
			return;

		timeAtLastFire = Time.time;

		//Vector3 offset = facingDirection;
		//GameObject projectile = Instantiate(projectilePrefab, transform.position + offset, Quaternion.identity);
		//projectile.GetComponent<Projectile>().direction = facingDirection;
		//projectile.GetComponent<Projectile>().owner = this;
		attack.Fire(facingDirection);
		AudioManager.instance.PlaySfxAtPoint(AudioManager.Sfx.Shoot, transform.position);
    }

	public void Hit(float dmg)
	{
		health -= dmg/def;
		if (health <= 0 && !isDead){ 
			isDead = true;
			Death();
		}

		AudioManager.instance.PlaySfxAtPoint(AudioManager.Sfx.Hit, transform.position);
	}

	public void Heal(float hp)
	{
		health += hp;
		health = Mathf.Min(health, baseHealth);
	}

	public virtual void Death() { }



}
