using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attacks;

public class Boss : Enemy
{
    public MultiShotAttack circleAttack;
    public MultiShotDelayAttack circleDelayAttack;
    public MultiShotDelayAttack quadCircleDelayAttack;

    public IAttack nextAttack;
    // Start is called before the first frame update
    void Start()
    {
        // Setup the attacks

        // Circle
        circleAttack = new MultiShotAttack
        {
            attacker = this,
            dmg = 1f,
            speed = 1f,
        };
        circleAttack.totalAngle = 360f;
        circleAttack.numberOfAttacks = 15;

        // Delay Circle
        circleDelayAttack = new MultiShotDelayAttack
        {
            attacker = this,
            dmg = 1f,
            speed = 1f,
        };
        circleDelayAttack.totalAngle = 360f;
        circleDelayAttack.numberOfAttacks = 15;
        nextAttack = circleAttack;

        // Quad Delay Circle
        quadCircleDelayAttack = new MultiShotDelayAttack
        {
            attacker = this,
            dmg = 1f,
            speed = 1f,
        };
        quadCircleDelayAttack.totalAngle = 360f;
        quadCircleDelayAttack.numberOfAttacks = 30;
        quadCircleDelayAttack.canonNumber = 4;
        quadCircleDelayAttack.delay = 250;

        nextAttack = circleAttack;
    }

    // Update is called once per frame
    void Update()
    {
	    distanceToTarget.x = transform.position.x - target.position.x;
		distanceToTarget.y = transform.position.y - target.position.y;
		bool xAlignedToTarget = Mathf.Abs (distanceToTarget.x) < 0.35f;
		bool yAlignedToTarget = Mathf.Abs (distanceToTarget.y) < 0.35f;

        Fire();
		CaclMovement(xAlignedToTarget, yAlignedToTarget);

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
	protected override void Fire ()
	{
		if (Time.time - timeAtLastFire < fireCooldown)
			return;

		timeAtLastFire = Time.time;

        nextAttack = ChoseAttack();

		nextAttack.Fire(facingDirection);
        
		AudioManager.instance.PlaySfxAtPoint(AudioManager.Sfx.Shoot, transform.position);
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

    private IAttack ChoseAttack() {
        int attackNumber = Random.Range(1,4);
        Debug.Log(attackNumber);
        switch(attackNumber)
        {
            // Circle attack
            case 1:

                Debug.Log("test1");
                fireCooldown = 1.5f;
                return circleAttack;

            // Circle delay attack
            case 2:

                Debug.Log("test2");
                fireCooldown = 3f;
                return circleDelayAttack;

            // Quad circle delay attack
            default:

                Debug.Log("test3");
                fireCooldown = 7.5f;
                return quadCircleDelayAttack;

        }
    }
}
