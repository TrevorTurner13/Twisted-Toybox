using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFearController : MonoBehaviour
{
    
    
    private float maxFear = 100.0f;
    private float currentFear;
    public float fearGainRate = 3.33f;
    public float fearLossRate = 33.3f;
    
    public float minOrthoSize = 3; // Minimum field of view
    public float maxOrthoSize = 9f; // Maximum field of view (less zoomed)
    public CinemachineVirtualCamera virtualCamera;

    private enum FearState
    {
        noFear,
        gainingFear,
        losingFear,
        fullFear
    }
    private FearState currentState = FearState.gainingFear;

    void Start()
    {
        if (virtualCamera == null)
        {
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case FearState.noFear:
                currentFear = 0;
                break;

            case FearState.gainingFear:
                currentFear += Time.deltaTime * fearGainRate;
                if(currentFear >= maxFear)
                {
                    currentState = FearState.fullFear;
                }
                break;

            case FearState.losingFear:
                currentFear -= Time.deltaTime * fearLossRate;
                if (currentFear <= 0)
                {
                    currentState = FearState.noFear;
                }
                break;

            case FearState.fullFear:
                currentFear = 100;               
                break;
        }

        float newOrthoSize = Mathf.Lerp(maxOrthoSize, minOrthoSize, currentFear / maxFear);
        virtualCamera.m_Lens.OrthographicSize = newOrthoSize;
       
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LightSource"))
        {
            currentState = FearState.losingFear;
        }
    }
   
    private void OnTriggerExit2D(Collider2D collision)
    {
       if (collision.CompareTag("LightSource"))
        {
            currentState = FearState.gainingFear;
        }
    }
}