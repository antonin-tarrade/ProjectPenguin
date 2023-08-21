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

		public Stats(int baseHealth, float speed, float attackSpeed, float dmg)
		{
			this.baseHealth = baseHealth;
			this.speed = speed;
			this.attackSpeed = attackSpeed;
			this.dmg = dmg;
		}
    }

	public void SetStats( Stats stats )
	{
		baseHealth = stats.baseHealth;
		speed = stats.speed;
		attack.dmg = stats.dmg;
		attack.speed = stats.attackSpeed;
	}

	[Serializable]
	public class StatModifier
	{
		public bool modifyHealth;
		public float healthModifier;
		public bool modifySpeed;
		public float speedModifier;
		public bool modifyAttackSpeed;
		public float attackSpeedModifier;
        public bool modifyDmg;
        public float dmgModifier;

		public void Apply(Penguin penguin)
		{
			if (modifyHealth) penguin.baseHealth = (int) (penguin.baseHealth * healthModifier);
			if (modifySpeed) penguin.speed *= speedModifier;
			if (modifyAttackSpeed) penguin.attack.speed *= attackSpeedModifier;
			if (modifyDmg) penguin.attack.dmg *= dmgModifier;
		}

    }

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

	// Competences
	public List<Upgrade> upgrades = new();
	public IAttack attack;

	// Pour ne pas que les ennemis se tuent entre eux
	public enum Type { Player, Ennemy }
	public Type type { get; protected set; }

    protected void InitPenguin ()
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

	protected void Fire ()
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
		health -= dmg;
		AudioManager.instance.PlaySfxAtPoint(AudioManager.Sfx.Hit, transform.position);
	}



}
