using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPullBlockController : MonoBehaviour
{
    [SerializeField] private float throwForce = 1.0f;
    private enum BlockState
    {
        notInInteractRange,
        inInteractRange,
        interacted,
        notInteracted
 
    }

    private PlayerMovement player;
    private FixedJoint2D joint;
    private Rigidbody2D selfRB;
    private bool playerInRange = false;
    

    [SerializeField]
    private BlockState currentState;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        selfRB = GetComponent<Rigidbody2D>();
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
                    player.carryPos.position = new Vector2(transform.position.x, transform.position.y + 1f);
                }
                break;

            case BlockState.interacted:               
                player.IsPushing = true;
                player.CurrentStance = PlayerMovement.playerStance.carrying;
                transform.SetParent(player.carryPos);
                transform.position = player.carryPos.position;
                selfRB.velocity = new Vector2(0, 0);
                if (player.IsGrabbing == false || player.IsGrounded()==false)
                {
                    currentState = BlockState.notInteracted;
                    if (player.IsFacingRight)
                    {
                        Throw(1);
                        playerInRange = false;
                    }
                    else
                    {
                        Throw(-1);
                        playerInRange = false;
                    }
                        
                    
                }
                break;

            case BlockState.notInteracted:
                player.IsPushing = false;
                player.CurrentStance = PlayerMovement.playerStance.standing;
                transform.SetParent(null);
                player.carryPos.position = player.DefaultTransformPOS;
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
            transform.SetParent(null);
        }
    }

    public void Throw(float xDir)
    {
        selfRB.velocity = throwForce * new Vector2(transform.localScale.x * xDir, 1);
        Debug.Log(xDir);
        currentState = BlockState.notInInteractRange;
        player.CurrentStance = PlayerMovement.playerStance.standing;
    }

  
}
