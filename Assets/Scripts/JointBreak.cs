using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointBreak : MonoBehaviour
{
    public HingeJoint2D joint;
    [SerializeField] private AudioSource soundFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            joint.enabled = false;
            if(soundFX != null)
            {
                soundFX.Play();
            }
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
