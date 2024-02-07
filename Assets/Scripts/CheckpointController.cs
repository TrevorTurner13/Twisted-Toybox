using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{

    private PlayerMovement player;
    private Transform respawnPoint;




    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        respawnPoint = transform.Find("Respawn Point");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Checkpoint Triggered");
           
            CheckpointManager.instance.SetCheckpoint(respawnPoint);
        }     
    }
}
