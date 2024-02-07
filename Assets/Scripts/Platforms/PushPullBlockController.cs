using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPullBlockController : MonoBehaviour
{

    private enum BlockState
    {
        notInInteractRange,
        inInteractRange,
        interacted,
        notInteracted
 
    }

    private PlayerMovement1 player;
    private HingeJoint2D hinge;
    private Rigidbody2D neutral;
    private bool playerInRange = false;

    [SerializeField]
    private BlockState currentState;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement1>();
        hinge = GetComponent<HingeJoint2D>();
        neutral = hinge.connectedBody;
    }
    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case BlockState.inInteractRange:
                if (player.IsGrabbing == true && player.IsGrounded())
                {
                    currentState= BlockState.interacted;
                }
                break;

            case BlockState.interacted:
                player.IsPushing = true;
                hinge.connectedBody = player.GetComponent<Rigidbody2D>();
                if (player.IsGrabbing == false || player.IsGrounded()==false)
                {
                    currentState = BlockState.notInteracted;
                }
                break;

            case BlockState.notInteracted:
                player.IsPushing = false;
                hinge.connectedBody = neutral;
                
                if (playerInRange == true)
                {
                    currentState = BlockState.inInteractRange;
                }
                else
                {
                    currentState = BlockState.notInInteractRange;
                }
                  
                break;

            case BlockState.notInInteractRange:
                
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentState = BlockState.inInteractRange;
            playerInRange = true;
        }       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && currentState != BlockState.interacted)
        {
            currentState = BlockState.notInInteractRange;
            playerInRange = false;
        }
    }
}
