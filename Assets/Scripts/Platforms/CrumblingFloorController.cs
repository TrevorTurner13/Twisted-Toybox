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
    private bool playerOnPlatform = false;
    private SpriteRenderer renderer;
    private BoxCollider2D collider;

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
                renderer.enabled = true;
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
                renderer.enabled = false;
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
            regenTimer = 0f;

            if (currentState == CrumblingState.PlatformBroken)
            {
                currentState = CrumblingState.PlatformRegenerating;
            }
        }
        
    }
}
