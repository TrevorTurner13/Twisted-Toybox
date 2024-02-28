using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrumblingFloorController : MonoBehaviour
{
    public float breakDelay;
    public float regenSpeed;

    private float breakTimer;
    private float regenTimer;
    private new SpriteRenderer renderer;
    private new BoxCollider2D collider;

   private enum CrumblingState
    {
        PlatformFixed,
        PlayerOnPlatform,       
        PlatformBroken,
        PlatformRegenerating
    }

    private CrumblingState currentState = CrumblingState.PlatformFixed;


    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        collider = transform.Find("Crumbling Platform Collider").GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case CrumblingState.PlatformFixed:
                renderer.enabled = true; //Re-Renders patform and enables the collision
                collider.enabled = true; 
                break;

            case CrumblingState.PlayerOnPlatform:
                breakTimer += Time.deltaTime;
                if (breakTimer >= breakDelay)
                {
                    currentState = CrumblingState.PlatformBroken;
                }
                break;

            case CrumblingState.PlatformBroken:                
                renderer.enabled = false;  //Hides platform and disables the collision
                collider.enabled = false;
                currentState = CrumblingState.PlatformRegenerating;
                break;
            
            case CrumblingState.PlatformRegenerating:
                regenTimer += Time.deltaTime;
                if(regenTimer >= regenSpeed)
                {
                    currentState = CrumblingState.PlatformFixed;
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            regenTimer = 0f;
            breakTimer = 0f;

            currentState = CrumblingState.PlayerOnPlatform;
        }       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            breakTimer = 0f;
            regenTimer = 0f;    //If the player leaves collider before platform breaks, just reset both timers
                                                                                
            if (currentState == CrumblingState.PlatformBroken)  //If player exits the collider due to the platform breaking, the platform begins to regen 
            {
                currentState = CrumblingState.PlatformRegenerating;
            }
        }
        
    }
}
