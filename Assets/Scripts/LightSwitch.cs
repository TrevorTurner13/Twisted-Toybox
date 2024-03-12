using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSwitch : MonoBehaviour
{
    private List<Light2D> lights = new List<Light2D>();
    private List<AudioSource> audioSources = new List<AudioSource>();
    private List<Collider2D> lightColliders =  new List<Collider2D>();

    void Start()
    {
        FindLights();
        FindAudioSources();
    }

    private void FindLights()
    {
        lights.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            for (int j = 0; j < transform.GetChild(i).transform.childCount; j++)
            {
                Light2D light = transform.GetChild(i).transform.GetChild(j).GetComponentInChildren<Light2D>();

                if (light != null)
                {
                    lights.Add(light);
                }

                Collider2D lightCollider = transform.GetChild(i).transform.GetChild(j).GetComponent<Collider2D>();

                if(lightCollider != null)
                {
                    lightColliders.Add(lightCollider);
                }
            }
        }
    }

    private void FindAudioSources()
    {
        audioSources.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            for (int j = 0; j < transform.GetChild(i).transform.childCount; j++)
            {
                AudioSource audio = transform.GetChild(i).transform.GetChild(j).GetComponentInChildren<AudioSource>();

                if (audio != null)
                {
                    audioSources.Add(audio);
                }
            }
        }
    }

    public void FlipSwitch()
    {
        foreach(Light2D light in lights)
        {
            light.gameObject.SetActive(!light.gameObject.activeSelf);
        }
        foreach (AudioSource audio in audioSources)
        {
            audio.Play();
        }
        foreach (Collider2D col in lightColliders)
        {
            col.enabled = !col.enabled;
        }
    }
  
}
