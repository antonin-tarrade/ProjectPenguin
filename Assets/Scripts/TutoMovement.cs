using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoMovement : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            AssignAnimations(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AssignAnimations(GameObject tuto){
        Animator animator = tuto.transform.GetChild(2).GetComponent<Animator>();
        switch(tuto.name){
            case "Left" :
                animator.SetTrigger("Left");
                break;
            case "Right" :
                animator.SetTrigger("Right");
                break;
            case "Up" :
                animator.SetTrigger("Up");
                break;
            case "Down" :
                animator.SetTrigger("Down");
                break;
            case "Shoot" :
                animator.SetTrigger("Shoot");
                break;
            case "Dash" : 
                animator.SetTrigger("Dash");
                break;
            case "Interact" :
                animator.SetTrigger("Interact");
                break;
        }
    }
}
