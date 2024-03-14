using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D.IK;
using TMPro;
using UnityEngine.UI;
using Cinemachine;
using UnityEditor.Experimental.GraphView;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Camera mainCamera;
    public LineRenderer lineRenderer;
    [SerializeField]
    public ButtonController currentInteractable;
    [SerializeField]
    private RopeHandler currentRope = null;
    public RopeHandler CurrentRope { get { return currentRope; } set { currentRope = value; } }
    [SerializeField]
    private DistanceJoint2D distanceJoint2D = null;
    [SerializeField] private GameObject fearCanvas;
    [SerializeField] private Rigidbody2D grapplePoint;

    public enum playerStance
    {
        standing,
        crouched,
        climbing,
        carrying,
        chased,
        cutscene,
        swinging,
        falling
    }

    private playerStance currentStance = playerStance.standing;
    public playerStance CurrentStance { get { return currentStance; } set {currentStance = value; } }

    public GameObject[] bodyParts;
    public LimbSolver2D[] limbSolvers;

    public Rigidbody2D emptyRB;
    public Transform carryPos;
    public Transform grabPos;
    private Vector2 defaultTransformPosition;

    private Vector2 climbSpeed;
   

    public Vector2 DefaultTransformPOS { get { return defaultTransformPosition; } }

    private float horizontal;
    private float vertical;
    private float speed = 3f;
    private float swingForce = 15f;
    private float chaseSpeed = 4f;
    private float defaultSpeed = 3f;
    private float defaultGravityScale = 1f;
    private float swingGravityScale = 3f;

    public float Speed { get { return speed; } set { speed = value; } }
    public float DefaultSpeed { get { return defaultSpeed; } }

    public float ChaseSpeed { get { return chaseSpeed; } } 
    public float DefaultGravityScale { get { return defaultGravityScale; } }
    public float SwingGravityScale { get { return swingGravityScale; } }
    private float jumpingPower = 6f;
    public bool isFacingRight = true;

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

    public LadderScript currentLadder;

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
            }
            else
            {
                if (!isClimbing) 
                {
                    animator.SetBool("isGrounded", false);
                }               
            }
            if(currentStance == playerStance.falling && IsGrounded())
            {
                currentStance = playerStance.standing;
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
                    speed = 2f;
                    rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
                    
                    if (rb.velocity.x != 0)
                    {
                        animator.SetBool("isWalking", false);
                        animator.SetBool("isCrouched", true);
                        animator.SetBool("isCrawling", true);
                        animator.SetBool("isSwinging", false);
                    }
                    else
                    {
                        animator.SetBool("isCrouched", true);
                        animator.SetBool("isCrawling", false);
                        animator.SetBool("isSwinging", false);
                    }
                    break;

                case playerStance.standing:
                    speed = defaultSpeed;
                    animator.speed = 1.0f;
                    rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
                    if (rb.velocity.x != 0)
                    {
                        animator.SetBool("onLadder", false);
                        animator.SetBool("isClimbing", false);
                        animator.SetBool("isCrouched", false);
                        animator.SetBool("isCrawling", false);
                        animator.SetBool("isWalking", true);
                        animator.SetBool("isCarrying", false);
                        animator.SetBool("isSwinging", false);
                    }
                    else
                    {
                        animator.SetBool("isCrouched", false);
                        animator.SetBool("isWalking", false);
                        animator.SetBool("isCarrying", false);
                        animator.SetBool("isSwinging", false);
                    }
                    break;

                case playerStance.climbing:
                    speed = 2f;
                    rb.velocity = new Vector2(rb.velocity.x * 0, vertical * speed);
                    isClimbing = true;
                    
                    if (rb.velocity.y != 0)
                    {
                        animator.SetBool("isClimbing", true);
                        animator.SetBool("isCrouched", false);
                        animator.SetBool("isCrawling", false);
                        animator.SetBool("isWalking", false);
                        animator.SetBool("onLadder", true);
                        animator.SetBool("isSwinging", false);
                    }
                    else
                    {
                        animator.SetBool("isClimbing", false);
                        animator.SetBool("onLadder", true);
                        animator.SetBool("isSwinging", false);
                    }
                    break;
                case playerStance.carrying:
                    {
                        speed = defaultSpeed;
                        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
                        if (rb.velocity.x != 0)
                        {
                            animator.SetBool("onLadder", false);
                            animator.SetBool("isClimbing", false);
                            animator.SetBool("isCrouched", false);
                            animator.SetBool("isCrawling", false);
                            animator.SetBool("isWalking", true);
                            animator.SetBool("isCarrying", true);
                            animator.SetBool("isSwinging", false);
                        }
                        else
                        {
                            animator.SetBool("isCrouched", false);
                            animator.SetBool("isWalking", false);
                            animator.SetBool("isCarrying", true);
                            animator.SetBool("isSwinging", false);
                        }
                        break;
                    }
                case playerStance.chased:
                    speed = chaseSpeed;
                    animator.speed = 1.2f;
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
                        animator.SetBool("isCarrying", false);
                    }
                    break;
                case playerStance.cutscene:
                    speed = 0f;
                    rb.velocity = new Vector2(0, 0);
                    animator.SetBool("onLadder", false);
                    animator.SetBool("isClimbing", false);
                    animator.SetBool("isCrouched", false);
                    animator.SetBool("isCrawling", false);
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isCarrying", false);
                    animator.SetBool("isSwinging", false);
                    break;
                case playerStance.swinging:
                    
                   if (horizontal != 0)
                    {
                        rb.AddForce(new Vector2(horizontal * swingForce, 0f));
                    }
                    //lineRenderer.SetPosition(1, emptyRB.transform.position);
                    animator.SetBool("onLadder", false);
                    animator.SetBool("isClimbing", false);
                    animator.SetBool("isCrouched", false);
                    animator.SetBool("isCrawling", false);
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isCarrying", false);
                    animator.SetBool("isSwinging", true);
                    break;
                case playerStance.falling:
                    if (horizontal != 0)
                    {
                        rb.AddForce(new Vector2(horizontal * swingForce, 0f));
                    }
                    animator.SetBool("onLadder", false);
                    animator.SetBool("isClimbing", false);
                    animator.SetBool("isCrouched", false);
                    animator.SetBool("isCrawling", false);
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isCarrying", false);
                    animator.SetBool("isSwinging", false);
                    break;
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
                if (isFacingRight)
                {
                    isFacingRight = false;
                }
                else
                {
                    isFacingRight = true;
                }
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
            else if( context.performed && inClimbRange && IsGrounded()) //when player interacts it checks if the interaction is with a ladder
            {
                if (currentLadder.isFacingRight && !isFacingRight)  //if player is facing left but ladder is facing right flip the player
                {
                    Flip();
                }
                else if (!currentLadder.isFacingRight && isFacingRight==true) //if player is facing right but ladder is facing left flip the player
                {                    
                    Flip();
                }
                transform.position = playerPlacement.position;
                rb.gravityScale = 0;             
                currentStance = playerStance.climbing;            //Changes players current stance to climbing, set gravity to 0 and resets vertical speed to zero to avoid unwanted vertical movement 
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
                    foreach (RopeScript rope in currentRope.ropes)
                    {
                        rope.GetComponent<Rigidbody2D>().drag = 0.5f;
                        rope.GetComponent<Rigidbody2D>().angularDrag = 0.5f;
                    }
                }
                distanceJoint2D.connectedBody = emptyRB;
                currentRope = null;
                transform.parent = null;
                currentStance = playerStance.falling;
                
                
            }
        }
    }

    public void RopeSwing(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Thwip");
        }
        if (context.performed && !IsGrounded())
        {
            Vector2 mousePos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(mousePos);
            lineRenderer.SetPosition(0, mousePos);
            lineRenderer.SetPosition(1, emptyRB.transform.position);
            distanceJoint2D.connectedBody = null;   
            distanceJoint2D.connectedAnchor = lineRenderer.GetPosition(0);
            lineRenderer.enabled = true;
            currentStance = playerStance.swinging;
            rb.drag = 0.1f;
            rb.angularDrag = 0.05f;
        }
        if (context.canceled)
        {
            distanceJoint2D.connectedBody = emptyRB; 
            distanceJoint2D.connectedAnchor = emptyRB.transform.position;
            lineRenderer.enabled = false;
            currentStance = playerStance.standing;
            rb.drag = 0f;
            rb.angularDrag = 0f;
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
        if (context.performed && currentStance == playerStance.standing || context.performed && currentStance == playerStance.chased)
        {
            currentStance = playerStance.crouched;
        }
        else if (context.performed && currentStance == playerStance.crouched)
        {
            currentStance = playerStance.standing;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            if (collision.GetComponent<ButtonController>() != null)
            {
                ButtonController button = collision.GetComponent<ButtonController>();
                currentInteractable = button;

                if (button.GetComponentInChildren<TextMeshPro>() != null) 
                {
                    button.GetComponentInChildren<TextMeshPro>().enabled = true;
                }
            }
        }
        //if (collision.CompareTag("Rope") && isGrabbing)
        //{
        //    if (collision.GetComponentInParent<RopeHandler>() != null && currentRope == null)
        //    {
        //        currentRope = collision.GetComponentInParent<RopeHandler>();
        //    }
        //}
        if (collision.CompareTag("Ladder"))
        {
            currentLadder = collision.GetComponent<LadderScript>();                    //When player enters ladders collision, find the ladders player placement            
            playerPlacement = collision.transform.Find("Player Placement").transform;  // and player is now in climbing range         
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
                if (button.GetComponentInChildren<TextMeshPro>() != null)
                {
                    button.GetComponentInChildren<TextMeshPro>().enabled = false;
                }
            }
        }
        else if (collision.CompareTag("Ladder"))
        {
            inClimbRange = false;  
            currentLadder = null;
            if (isClimbing)
            {               
                playerPlacement = null;          //When player leaves a ladder after climbing, reset all variables related to climbing
                isClimbing = false;
                rb.gravityScale = DefaultGravityScale;
                currentStance = playerStance.standing;
                rb.AddForce(Vector2.up * 2, ForceMode2D.Impulse); //Adds an upwards boost to the player to help getting onto next platform
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