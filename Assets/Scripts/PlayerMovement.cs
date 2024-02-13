using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D.IK;
using TMPro;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField]
    public ButtonController currentInteractable;
    [SerializeField]
    private RopeHandler currentRope = null;
    [SerializeField]
    private DistanceJoint2D distanceJoint2D = null;
    [SerializeField] private GameObject fearCanvas;


    public enum playerStance
    {
        standing,
        crouched,
        climbing,
        carrying
    }

    private playerStance currentStance = playerStance.standing;
    public playerStance CurrentStance { get { return currentStance; } set { value = currentStance; } }

    public GameObject[] bodyParts;
    public LimbSolver2D[] limbSolvers;

    public Rigidbody2D emptyRB;
    public Transform carryPos;
    private Vector2 defaultTransformPosition;

    private Vector2 climbSpeed;
   

    public Vector2 DefaultTransformPOS { get { return defaultTransformPosition; } }

    private float horizontal;
    private float vertical;
    private float speed = 3f;
    private float swingSpeed = 8f;
    private float defaultSpeed = 3f;
    private float defaultGravityScale = 1f;
    private float swingGravityScale = 1f;

    public float Speed { get { return speed; } set { speed = value; } }
    public float SwingSpeed { get { return swingSpeed; } }
    public float DefaultSpeed { get { return defaultSpeed; } }
    public float DefaultGravityScale { get { return defaultGravityScale; } }
    public float SwingGravityScale { get { return swingGravityScale; } }
    private float jumpingPower = 6f;
    private bool isFacingRight = true;

    public bool IsFacingRight { get { return isFacingRight; } }
    private bool isGrabbing = false;

    private bool isPushing = false;
    public bool IsPushing { get { return isPushing; } set { isPushing = value; } }

    private bool isPaused = false;

    public bool IsPaused { get { return isPaused; } set { isPaused = value; } }

    public bool IsGrabbing { get { return isGrabbing; } }

    public bool isDead = false;
    public bool isDying = false;
    public bool isClimbing = false;
    private bool inClimbRange = false;
    private Transform playerPlacement = null; 

    private void Start()
    {
        fearCanvas.SetActive(true);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        distanceJoint2D = GetComponent<DistanceJoint2D>();
        defaultTransformPosition = carryPos.position;
        if (CheckpointManager.instance.LastCheckpointPosition != null)
        {
            transform.position = CheckpointManager.instance.LastCheckpointPosition;
        }
        climbSpeed = new Vector2(rb.velocity.x * 0, vertical * speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDying)
        {
            

            if (!isPaused)
            {
                if (!isFacingRight && horizontal > 0)
                {
                    Flip();
                }
                else if (isFacingRight && horizontal < 0)
                {
                    Flip();
                }
            }
            if (IsGrounded())
            {
                animator.SetBool("isGrounded", true);
                speed = defaultSpeed;
            }
            else
            {
                if (!isClimbing) 
                {
                    animator.SetBool("isGrounded", false);
                }               
            }
            
            //if(isPushing)
            //{
            //    currentStance = playerStance.carrying;
            //}
            //else
            //{
            //    currentStance = playerStance.standing;
            //}
            switch (currentStance)
            {
                case playerStance.crouched:
                    rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
                    if (rb.velocity.x != 0)
                    {
                        animator.SetBool("isWalking", false);
                        animator.SetBool("isCrouched", true);
                        animator.SetBool("isCrawling", true);
                    }
                    else
                    {
                        animator.SetBool("isCrouched", true);
                        animator.SetBool("isCrawling", false);
                    }
                    break;

                case playerStance.standing:
                    rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
                    if (rb.velocity.x != 0)
                    {
                        animator.SetBool("onLadder", false);
                        animator.SetBool("isClimbing", false);
                        animator.SetBool("isCrouched", false);
                        animator.SetBool("isCrawling", false);
                        animator.SetBool("isWalking", true);
                        animator.SetBool("isCarrying", false);
                    }
                    else
                    {
                        animator.SetBool("isCrouched", false);
                        animator.SetBool("isWalking", false);
                    }
                    break;

                case playerStance.climbing:                   
                    rb.velocity = new Vector2(rb.velocity.x * 0, vertical * speed);
                    isClimbing = true;
                    
                    if (rb.velocity.y != 0)
                    {
                        animator.SetBool("isClimbing", true);
                        animator.SetBool("isCrouched", false);
                        animator.SetBool("isCrawling", false);
                        animator.SetBool("isWalking", false);
                        animator.SetBool("onLadder", true);
                    }
                    else
                    {
                        animator.SetBool("isClimbing", false);
                        animator.SetBool("onLadder", true);
                    }
                    break;
                case playerStance.carrying:
                    {
                        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
                        animator.SetBool("isCarrying", true);
                        break;
                    }
            }

        }
        else if (isDead)
        {
            animator.SetBool("isDead", true);
        }
        else if (isDying)
        {
           animator.SetBool("isDying", true);
        }
    }

    public void MakeRagdoll()
    {
        animator.enabled = false;
        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        collider.size = new Vector2(collider.size.x, 0.3f);
        for (int i = 0; i < limbSolvers.Length; i++)
        {
            limbSolvers[i].enabled = false;
        }
        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].GetComponent<Rigidbody2D>().isKinematic = false;
            HingeJoint2D hinge = bodyParts[i].GetComponent<HingeJoint2D>();
            if (hinge != null)
            {
                bodyParts[i].GetComponent<HingeJoint2D>().enabled = true;
            }
            bodyParts[i].GetComponent<Collider2D>().enabled = true;
        }
        isDead = true;
        isDying = true;
    }

    public void BreakBody()
    {
        for (int i = 0; i < bodyParts.Length; i++)
        {
            HingeJoint2D hinge = bodyParts[i].GetComponent<HingeJoint2D>();
            if (hinge != null)
            {
                bodyParts[i].GetComponent<HingeJoint2D>().enabled = false; ;
            }
        }
    }

    private void StopRagdoll()
    {
        animator.enabled = true;
        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        collider.size = new Vector2(collider.size.x, 1.8f); ;
        for (int i = 0; i < limbSolvers.Length; i++)
        {
            limbSolvers[i].enabled = true;
        }
        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].GetComponent<Rigidbody2D>().isKinematic = true;
            HingeJoint2D hinge = bodyParts[i].GetComponent<HingeJoint2D>();
            if (hinge != null)
            {
                bodyParts[i].GetComponent<HingeJoint2D>().enabled = false;
            }
            bodyParts[i].GetComponent<Collider2D>().enabled = false;
        }
    }
    private void FixedUpdate()
    {

    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!isDying && !isClimbing)
        {
            if (context.performed && IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);


            }
            if (context.canceled && rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }

        }

    }

    public bool IsGrounded()
    {

        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (!isDying)
        {
            if (isClimbing == false)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!isDying)
        {
            if (isClimbing == false)
            {
                
                horizontal = context.ReadValue<Vector2>().x;
            }
            else
            {
                vertical = context.ReadValue<Vector2>().y;
            }
                   
            
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (!isDying)
        {
            if (context.performed && currentInteractable != null)
            {
                currentInteractable.Interact();
                
            }
            else if( context.performed && inClimbRange && IsGrounded())
            {
                transform.position = playerPlacement.position;
                rb.gravityScale = 0;
                Debug.Log("startClimb");
                currentStance = playerStance.climbing;
                vertical = 0;
            }

        }

    }

    public void Grab(InputAction.CallbackContext context)
    {
        if (!isDying)
        {
            if (context.performed)
            {
                isGrabbing = true;
            }
            if (context.canceled)
            {
                isGrabbing = false;

                if (currentRope != null)
                {
                    currentRope.Grabbed = false;
                    distanceJoint2D.connectedBody = emptyRB;
                    speed = 4f;
                    GetComponent<Rigidbody2D>().gravityScale = defaultGravityScale;
                    currentRope = null;
                    transform.parent = null;
                    animator.SetBool("isSwinging", false);
                    currentStance = playerStance.standing;
                }
            }
        }
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PauseManager.instance.PauseGame();
            isPaused = true;
        }

    }

    public void Crouch (InputAction.CallbackContext context)
    {
        if (context.performed && currentStance == playerStance.standing)
        {
            currentStance = playerStance.crouched;
        }
        else if (context.performed && currentStance == playerStance.crouched)
        {
            currentStance = playerStance.standing;
        }
    }


    //public void Unpause(InputAction.CallbackContext context)
    //{
    //    if (isPaused && Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        if (context.performed)
    //        {
    //            PauseManager.instance.UnpauseGame();
    //            isPaused = false;
    //        }
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            if (collision.GetComponent<ButtonController>() != null)
            {
                ButtonController button = collision.GetComponent<ButtonController>();
                currentInteractable = button;

                button.GetComponentInChildren<TextMeshPro>().enabled = true;
            }
        }
        if (collision.CompareTag("Rope") && isGrabbing)
        {
            if (collision.GetComponentInParent<RopeHandler>() != null)
            {
                currentRope = collision.GetComponentInParent<RopeHandler>();
                Debug.Log(currentRope);
                animator.SetBool("isSwinging", true);
                
            }
        }
        if (collision.CompareTag("Ladder"))
        {
            playerPlacement = collision.transform.Find("Player Placement").transform;
             
            inClimbRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            currentInteractable = null;

            if (collision.GetComponent<ButtonController>() != null)
            {
                ButtonController button = collision.GetComponent<ButtonController>();
                button.GetComponentInChildren<TextMeshPro>().enabled = false;
            }
        }
        else if (collision.CompareTag("Ladder"))
        {
            inClimbRange = false;
            if (isClimbing)
            {
                playerPlacement = null;
                isClimbing = false;
                rb.gravityScale = DefaultGravityScale;
                currentStance = playerStance.standing;
                rb.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
            }
        }
    }

    public void KillPlayer()
    {
        isDying = true;

        GameOverManager.instance.GameOverPanelActive();
    }

    public void DyingToDeadTransition()
    {
        isDead = true;
    }

    
}