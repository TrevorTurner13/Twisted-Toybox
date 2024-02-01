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
            connectPoint = _handler.FindNearestRopePoint(player.transform);
            player.transform.SetParent(connectPoint);
            player.transform.position = connectPoint.position;
            connectPoint.gameObject.AddComponent<FixedJoint2D>();
            connectPoint.gameObject.GetComponent<FixedJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
            player.Speed = player.SwingSpeed;
            player.GetComponent<Rigidbody2D>().gravityScale = player.SwingGravityScale;
            _handler.Grabbed = true;
        }
    }
   
}
