using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Player : Penguin
{
    //Moche
    private bool hasSecondChance = false;

    //Doit pas etre la
    public Vector3 spawnPoint;

    [Header("FireParameters")]
    [SerializeField] private float fireRange;
    [SerializeField] private float maxShootAngle;
    [SerializeField] private int enemyDetectionPrecision;

    public static int iceShards;
    public int score;
    public float fishingTime = 10f;

    public Upgrade[] upgradesList;
    public Health healthUI;

    private PlayerController controller;

    // Start is called before the first frame update
    void Start()
    {
        InitPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        controller.HandleInput();
        Animate();
        Move();
    }


    public override void InitPenguin()
    {
        base.InitPenguin();
        SetStats(GameManager.instance.battleData.playerStats);
        health = baseHealth;
    }

    public void InitPlayer()
    {
        InitPenguin();
        type = Type.Player;
        GameManager.instance.playerRespawnEvent += Respawn;

        upgradesList = new Upgrade[]{
            new SpeedUpgrade(),
            new StrengthUpgrade(),
            new HealthUpgrade(healthUI),
            new SlowShotUpgrade(),
            new MultishotUpgrade(),
            new SecondChanceUpgrade(),
            new SlidingUpgrade(),
            new FishingUpgrade(),
        };

        controller = new PlayerController(this);
    }


    public void Move(Vector2 direction)
    {
        movement = direction.normalized * speed;
        if (direction.sqrMagnitude > 0) facingDirection = direction;
    }

    public void StartSlide()
    {
        isSliding = true;
        StartCoroutine("Slide");
    }

    public void StopSlide()
    {
        isSliding = false;
        StopCoroutine("Slide");
    }

    public new void Fire()
    {
        Enemy closestEnemy = FindClosestEnnemyInRange();
        if (closestEnemy == null)
        {
            attack.Fire(facingDirection);
        }
        else
        {
            attack.Fire(closestEnemy.transform.position - transform.position);
        }
    }

    private Enemy FindClosestEnnemyInRange()
    {
        RaycastHit2D[] raycasts = new RaycastHit2D[enemyDetectionPrecision];
        float stepAngle = 2 * maxShootAngle / enemyDetectionPrecision;
        Vector2 direction = Rotate(facingDirection.normalized, maxShootAngle * Mathf.Deg2Rad);

        float smallestDistance = float.MaxValue;
        Enemy closestEnemy = null;
        Enemy enemy = null;

        for (int i = 0; i < enemyDetectionPrecision; i++)
        {
            direction = Rotate(direction, -stepAngle * Mathf.Deg2Rad);

            raycasts[i] = Physics2D.Raycast(transform.position, direction, fireRange, LayerMask.GetMask("Enemies", "Obstacles"));
            Debug.DrawLine(transform.position, raycasts[i].point);
            if (raycasts[i])
            {
                if (raycasts[i].distance < smallestDistance)
                {
                    //Debug.Log("DISTANCE");
                    if (raycasts[i].collider.gameObject.TryGetComponent<Enemy>(out enemy))
                    {
                        //Debug.Log("ENEMY");
                        closestEnemy = enemy;
                    }
                }
            }
        }
        return closestEnemy;
    }

    public Vector2 Rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
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

    private void Animate()
    {
        if (movement.y < 0) // Down
            animator.SetInteger("orientation", 0);
        if (movement.x < 0) // Left
            animator.SetInteger("orientation", 1);
        if (movement.y > 0) // Up
            animator.SetInteger("orientation", 2);
        if (movement.x > 0) // Right
            animator.SetInteger("orientation", 3);
        animator.SetBool("isSliding", isSliding);
        animator.SetFloat("speed", movement.magnitude);
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

    public void Respawn()
    {
        if (hasSecondChance)
        {
            hasSecondChance = false;
            isDead = false;
            health = baseHealth;
            transform.position = spawnPoint;
        }
        else
        {
            SceneManager.LoadScene("SampleScene");
        }
    }



}





















public class PlayerController
{
    public bool enabled;

    public IPlayerState state;
    public PlayerController(Player player)
    {
        state = new PlayerMovingState(player);
        enabled = true;
    }
    public void HandleInput()
    {
        if (!enabled) return;
        state = state.HandleInput();
    }

    public void Enable() { enabled = true; }
    public void Disable() { enabled = false; }


}

public interface IPlayerState
{
    public string GetName();
    public IPlayerState HandleInput();
}

public abstract class PlayerState : IPlayerState
{
    public string name { get; protected set; }

    protected Player player;

    public string GetName()
    {
        return name;
    }

    public abstract IPlayerState HandleInput();


    public PlayerState(Player player)
    {
        this.player = player;
    }
}

public class PlayerMovingState : PlayerState
{
    private Vector2 movement;

    public PlayerMovingState(Player player) : base(player)
    {
        name = "Moving";
        movement = new();
    }

    public override IPlayerState HandleInput()
    {
        if (Input.GetKeyDown("space"))
        {
            player.StartSlide();
            return new PlayerSlidingState(player);
        }
        else
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            player.Move(movement);
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.R))
            {
                player.Fire();
            }
            return this;
        }
    }
}

public class PlayerSlidingState : PlayerState
{
    public PlayerSlidingState(Player player) : base(player)
    {
        name = "Sliding";
    }

    public override IPlayerState HandleInput()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            player.StopSlide();
            return new PlayerMovingState(player);
        }
        else return this;
    }
}


