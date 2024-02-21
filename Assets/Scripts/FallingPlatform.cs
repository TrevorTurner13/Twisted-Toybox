using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    
    public void DropPlatform(float value)
    {
         rigidBody.isKinematic = false;
        StartCoroutine(StickPlatform(value));
    }

    IEnumerator StickPlatform(float value)
    {
        yield return new WaitForSeconds(value);
        rigidBody.isKinematic = true;
    }
}
