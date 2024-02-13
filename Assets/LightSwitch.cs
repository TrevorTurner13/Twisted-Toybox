using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSwitch : MonoBehaviour
{
    private List<Light2D> lights = new List<Light2D>();
    private List<Collider2D> lightColliders =  new List<Collider2D>();

    void Start()
    {
        FindLights();
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

    public void FlipSwitch()
    {
        foreach(Light2D light in lights)
        {
            light.gameObject.SetActive(!light.gameObject.activeSelf);
        }
        foreach (Collider2D col in lightColliders)
        {
            col.enabled = !col.enabled;
        }
    }
  
}
