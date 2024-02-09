using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : MonoBehaviour
{
    [SerializeField] private Rigidbody2D[] rbs;
    private PlayerMovement player;
    private Collider2D col;

    private void Start()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            col.enabled = false;
            foreach (var rb in rbs)
            {
                rb.GetComponent<Rigidbody2D>().isKinematic = false;
                
            }
            player = collision.GetComponent<PlayerMovement>();
            player.MakeRagdoll();            
            player.BreakBody();
        }
    }
}
