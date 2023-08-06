using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{

    public float speed = 5f;
    public float slideBoost = 2f;
    public float slideSlowDown = 0.75f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D body;

    private float movement_x = 0;
    private float movement_y = 0;
    
    // Booleens pour empecher les mouvements diagonaux
    private bool movement_x_lock;
    private bool movement_y_lock;

    private bool isSliding;



    void Start(){
        this.animator = GetComponent<Animator>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.body = GetComponent<Rigidbody2D>();

        movement_x_lock = false;
        movement_y_lock = false;
        isSliding = false;

        animator.SetFloat("speed", 0);
        animator.SetInteger("orientation", 0);
    }

    void Update(){
        getMovementInput();

        animator.SetFloat("speed", Mathf.Abs(movement_x) + Mathf.Abs(movement_y));
        if (movement_y < 0) // Down
            animator.SetInteger("orientation", 0);
        if (movement_x < 0) // Left
            animator.SetInteger("orientation", 1);
        if (movement_y > 0) // Up
            animator.SetInteger("orientation", 2);
        if (movement_x > 0) // Right
            animator.SetInteger("orientation", 3);
        

        if (Input.GetKeyDown("space")){
            isSliding = true;
            movement_x = slideBoost*movement_x;
            movement_y = slideBoost*movement_y;
            StartCoroutine(slidingCoroutine());
        }
        if (Input.GetKeyUp("space") || Mathf.Abs(movement_x) + Mathf.Abs(movement_y) < 0.01)
            isSliding = false;
        
        animator.SetBool("isSliding", isSliding);
    }

    void FixedUpdate(){

        body.velocity = new Vector2(movement_x * speed,
                                     movement_y * speed);
    }

    private void getMovementInput(){

        if (!(movement_x_lock||isSliding))
            movement_x = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(movement_x)>0.01)
            movement_y_lock = true;

        if (!(movement_y_lock||isSliding))
            movement_y = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(movement_y)>0.01)
            movement_x_lock = true;

        if (Mathf.Abs(movement_x) < 0.01)
            movement_y_lock = false;
        if (Mathf.Abs(movement_y) < 0.01)
            movement_x_lock = false;

    }

    IEnumerator slidingCoroutine()
    {
        while (isSliding)
        {
            movement_x = slideSlowDown*movement_x;
            movement_y = slideSlowDown*movement_y;

            if (Mathf.Abs(movement_x) + Mathf.Abs(movement_y) < 0.1)
                isSliding = false;

            yield return new WaitForSeconds(0.5f);
        }
    }

}
