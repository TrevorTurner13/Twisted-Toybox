using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotingPatformController : MonoBehaviour
{
    public float forceMagnitude = 10f; // Adjust the force magnitude

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            // Calculate direction and position for the force application
            Vector2 forceDirection = (collision.transform.position.x < transform.position.x) ? Vector2.right : Vector2.left;
            Vector2 forcePoint = collision.transform.position;

            // Apply the force
            rb.AddForceAtPosition(forceDirection * forceMagnitude, forcePoint);
        }
    }
}
