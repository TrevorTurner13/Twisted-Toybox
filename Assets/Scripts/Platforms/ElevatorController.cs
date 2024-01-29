using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public Transform destination;
    public float speed;
    public float startDelay;

    private enum ElevatorState
    {
        MovingToDestination,
        MovingToStart,
        AtStart,
        AtDestination
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

            case ElevatorState.AtDestination:

                break;

            case ElevatorState.MovingToDestination:
                delayTimer += Time.deltaTime;
                if (delayTimer >= startDelay)
                {
                    MoveElevator(destination.position);
                    if (transform.position == destination.position)
                    {
                        currentState = ElevatorState.AtDestination;
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
            currentState = ElevatorState.MovingToDestination;
            delayTimer = 0f; // Reset delay timer for the next state
        }
        if (collision.CompareTag("Player") && currentState == ElevatorState.AtDestination)
        {
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
        if (collision.CompareTag("Player") && currentState == ElevatorState.MovingToDestination)  //If player pxits elevator while it is moving the elevator will always go back to the starting position
        {
            collision.transform.SetParent(null);
            currentState = ElevatorState.MovingToStart;
            delayTimer = 0f; 
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
        else if (target == "Destination")
        {
            currentState = ElevatorState.MovingToDestination;
        }
    }
}










