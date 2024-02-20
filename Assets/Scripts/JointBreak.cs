using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointBreak : MonoBehaviour
{
    public HingeJoint2D joint;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision");

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Falling body");
            joint.enabled = false;

        }
    }
}
