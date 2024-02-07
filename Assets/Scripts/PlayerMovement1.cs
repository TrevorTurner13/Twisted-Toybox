using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D.IK;

public class PlayerMovement1 : MonoBehaviour
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
    private Transform currentGrabPoint = null;

    
    public GameObject[] bodyParts;
    public LimbSolver2D[] limbSolvers;
    public Transform handPos;

    private float horizontal;
    private float speed = 3f;
    private float swingSpeed = 10f;
    private float defaultSpeed = 3f;
    private float defaultGravityScale = 1f;
    private float swingGravityScale = 3f;
    public float Speed { get { return speed; } set { speed = value; } }
    public float SwingSpeed { get { return swingSpeed; } }
    public float DefaultSpeed { get { return defaultSpeed; } }
    public float DefaultGravityScale { get { return defaultGravityScale; } }
    public float SwingGravityScale {  get { return swingGravityScale; } }
    private float jumpingPower = 6f;
    private bool isFacingRight = true;
    private bool isGrabbing = false;

    private bool isPushing = false;
    public bool IsPushing { get { return isPushing; } set { isPushing = value; } }

    private bool isPaused = false;
    public bool IsGrabbing {  get { return isGrabbing; } }

    public bool isDead = false;
    public bool isDying = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            MakeRagdoll();
        }
        if (!isDying)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

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
                animator.SetBool("isGrounded", false);
            }
            if (rb.velocity.x != 0)
            {
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
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

    private void MakeRagdoll()
    {
        animator.enabled = false;
        //GetComponent<Collider2D>().enabled = false;
        for (int i = 0; i < limbSolvers.Length; i++)
        {
            limbSolvers[i].enabled = false;
        }
        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].GetComponent<Rigidbody2D>().isKinematic = false;
            bodyParts[i].GetComponent<HingeJoint2D>().enabled = true;
            //bodyParts[i].GetComponent<Collider2D>().enabled = true;
        }
    }
    private void FixedUpdate()
    {
        
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!isDying)
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
    {if (!isDying)
        {
            if (isPushing == false)
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
            horizontal = context.ReadValue<Vector2>().x;
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
        }
        
    }

    public void Grab(InputAction.CallbackContext context)
    {
        if (!isDying)
        {
            if (context.performed)
            {
                isGrabbing = true;
                Debug.Log("Grabbed");
            }
            if (context.canceled)
            {
                Debug.Log("NoGrab");
                isGrabbing = false;

                if (currentRope != null)
                {

                    if (currentGrabPoint != null)
                    {
                        Destroy(currentGrabPoint.gameObject.GetComponent<FixedJoint2D>());
                        currentGrabPoint = null;

                    }
                    currentRope.Grabbed = false;
                    speed = 6f;
                    GetComponent<Rigidbody2D>().gravityScale = defaultGravityScale;
                    currentRope = null;
                    transform.parent = null;
                    animator.SetBool("isSwinging", false);
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
            }            
        }
        if (collision.CompareTag("Rope") && isGrabbing)
        {
            if (collision.GetComponentInParent<RopeHandler>() != null)
            {
                currentRope = collision.GetComponentInParent<RopeHandler>();
                currentGrabPoint = currentRope.FindNearestRopePoint(transform);
                animator.SetBool("isSwinging", true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            currentInteractable = null;
        }
    }
    
    public void KillPlayer()
    {
        isDying = true; 
    }
    public void DyingToDeadTransition()
    {
        isDead = true;
    }

}
