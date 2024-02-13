using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public Transform destination1;
    public Transform destination2;
    public float speed;
    public float startDelay;

    private enum ElevatorState
    {
        MovingToDestination1,
        MovingToDestination2,
        MovingToStart,
        AtStart,
        AtDestination1,
        AtDestination2
    }

    private ElevatorState currentState = ElevatorState.AtStart;
    private Vector3 startPosition;
    private float delayTimer;

    void Start()
    {
        startPosition = transform.position;

    }

    void Update()
    {
        switch (currentState)
        {
            case ElevatorState.AtStart:

                break;

            case ElevatorState.AtDestination1:

                break;

            case ElevatorState.AtDestination2:
                
                break;

            case ElevatorState.MovingToDestination1:
                delayTimer += Time.deltaTime;
                if (delayTimer >= startDelay)
                {
                    MoveElevator(destination1.position);
                    if (transform.position == destination1.position)
                    {
                        currentState = ElevatorState.AtDestination1;
                        delayTimer = 0f;
                    }
                }
                break;

            case ElevatorState.MovingToDestination2:
                delayTimer += Time.deltaTime;
                if (delayTimer >= startDelay)
                {
                    MoveElevator(destination2.position);
                    if (transform.position == destination2.position)
                    {
                        currentState = ElevatorState.AtDestination2;
                        delayTimer = 0f;
                    }
                }
                break;

            case ElevatorState.MovingToStart:
                delayTimer += Time.deltaTime;
                if (delayTimer >= startDelay)
                {
                    MoveElevator(startPosition);
                    if (transform.position == startPosition)
                    {
                        currentState = ElevatorState.AtStart; 
                        delayTimer = 0f;
                    }
                }
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && currentState == ElevatorState.AtStart)
        {
            collision.transform.SetParent(transform);
            currentState = ElevatorState.MovingToDestination1;
            delayTimer = 0f; // Reset delay timer for the next state
        }
        if (collision.CompareTag("Player") && currentState == ElevatorState.AtDestination1)
        {
            collision.transform.SetParent(transform);
            currentState = ElevatorState.MovingToDestination2;
            delayTimer = 0f; // Reset delay timer for the next state
        }
        if (collision.CompareTag("Player") && currentState == ElevatorState.AtDestination2)
        {
            collision.transform.SetParent(transform);
            currentState = ElevatorState.MovingToStart;
            delayTimer = 0f; // Reset delay timer for the next state
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && currentState == ElevatorState.MovingToStart)
        {
            collision.transform.SetParent(null);
            currentState = ElevatorState.MovingToStart;
            delayTimer = 0f; 
        }
        if (collision.CompareTag("Player") && currentState == ElevatorState.MovingToDestination1)  //If player pxits elevator while it is moving the elevator will always go back to the starting position
        {
            collision.transform.SetParent(null);
            currentState = ElevatorState.MovingToStart;
            delayTimer = 0f; 
        }
        else if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }

    }

    private void MoveElevator(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    public void ActivateElevator(string target) //In OnPressed in the button input either Start or Destination depending on desired location
    {
        if (target == "Start")
        {
            currentState = ElevatorState.MovingToStart;
        }
        else if (target == "Destination1")
        {
            currentState = ElevatorState.MovingToDestination1;
        }
        else if (target == "Destination2")
        {
            currentState = ElevatorState.MovingToDestination2;
        }
    }
}










