using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class RopeScript : MonoBehaviour
{
    [SerializeField]
    private RopeHandler _handler;

    [SerializeField]
    PlayerMovement player;

    private Transform connectPoint;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_handler.Grabbed && collision.CompareTag("Player") && player.IsGrabbing)
        {
            connectPoint = _handler.FindNearestRopePoint(player.emptyRB.transform);
            player.gameObject.GetComponent<DistanceJoint2D>().connectedBody = connectPoint.GetComponent<Rigidbody2D>();
            player.Speed = player.SwingSpeed;
            player.GetComponent<Rigidbody2D>().gravityScale = player.SwingGravityScale;
            _handler.Grabbed = true;
        }
    }

}