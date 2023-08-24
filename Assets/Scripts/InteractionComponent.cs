using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class InteractionComponent : MonoBehaviour
{
    [SerializeField]
    private List<string> triggerTags;

    [SerializeField]
    private KeyCode triggerKey;

    private bool isActive = false;

    private int triggerCounter = 0;

    public UnityEvent interactionEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggerTags.Contains(collision.tag))
        {
            isActive = true;
            triggerCounter++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (triggerTags.Contains(collision.tag))
        {
            triggerCounter--;
            if (triggerCounter == 0)
            {
                isActive = false;
            }
        }
    }

    private void Update()
    {
        if (isActive && Input.GetKeyDown(triggerKey))
        {
            interactionEvent.Invoke();
        }
    }
}
