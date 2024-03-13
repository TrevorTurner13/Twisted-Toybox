using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeAltScript : MonoBehaviour
{
    private DistanceJoint2D ropeJoint;
    private PlayerMovement player;

    private void Start()
    {
        ropeJoint = GetComponent<DistanceJoint2D>();
        player = FindObjectOfType<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && player.IsGrabbing)
        {
            ropeJoint.connectedBody = player.GetComponent<Rigidbody2D>();
            player.transform.parent = transform.parent;
            player.CurrentStance = PlayerMovement.playerStance.swinging;
        }
    }
}
