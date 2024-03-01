using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFearController : MonoBehaviour
{
    
    
    private float maxFear = 100.0f;
    private float currentFear;
    public float fearGainRate = 1f;
    public float fearLossRate = 33.3f;
    
    public float minOrthoSize = 3; // Minimum field of view
    public float maxOrthoSize = 9f; // Maximum field of view (less zoomed)
    public CinemachineVirtualCamera virtualCamera;

    public Image fearOverlay;
    public Image secondFearOverlay;

    public AudioSource fearAudio;
    private float fearVolume;
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
        fearAudio.volume = 0;
     
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
        //Change camera zoom depending on fear level
        float newOrthoSize = Mathf.Lerp(maxOrthoSize, minOrthoSize, currentFear / maxFear);
        virtualCamera.m_Lens.OrthographicSize = newOrthoSize;

        //Change transparency of fearoverlay depending on fear level
        AdjustFearOverlay(fearOverlay);
        AdjustFearOverlay(secondFearOverlay);

      if ( currentFear > 30)
        {
            fearVolume = Mathf.Lerp(0.0f, 0.1f, (currentFear - 30) / maxFear);
        }
        else
        {
            fearVolume = 0.0f;
        }   
            fearAudio.volume = fearVolume;
        
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
    
    private void AdjustFearOverlay(Image layer)
    {
        if (layer != null)
        {
            Color overlayColor = layer.color;
            overlayColor.a = Mathf.Lerp(0f, 1f, currentFear / maxFear);
            layer.color = overlayColor;
        }
    }
}
