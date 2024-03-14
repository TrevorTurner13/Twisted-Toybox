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

    private void Start()
    {
        player = FindAnyObjectByType<PlayerMovement>();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_handler.Grabbed && collision.CompareTag("Player") && player.CurrentRope == null && player.IsGrabbing)
        {
            connectPoint = _handler.FindNearestRopePoint(player.emptyRB.transform);
            player.transform.parent = connectPoint.parent;
            player.CurrentStance = PlayerMovement.playerStance.swinging;
            player.CurrentRope = _handler;
            player.gameObject.GetComponent<DistanceJoint2D>().connectedBody = connectPoint.GetComponent<Rigidbody2D>();
            _handler.Grabbed = true;
            foreach (RopeScript rope in _handler.ropes)
            {
                rope.GetComponent<Rigidbody2D>().angularDrag = 0;
                rope.GetComponent<Rigidbody2D>().drag = 0;
            }
        }
    }

}