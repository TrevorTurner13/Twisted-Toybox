using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class LightFlickerEffect : MonoBehaviour
{
    [Tooltip("External light to flicker; you can leave this null if you attach the script to a light")]
    //public new Light light;
    public new Light2D light;

    //public float minIntensity = 0f;
    //public float maxIntensity = 1f;
    //[Range(1, 50)]
    //public int smoothing = 5;

    //Queue<float> smoothQueue;
    //float lastSum = 0;

    public float flickerDelay = 0.8f;

    float nextFlickerTime;
    float timer;

    void Start()
    {
        //smoothQueue = new Queue<float>(smoothing);
        if (light == null)
            light = GetComponent<Light2D>();

        //nextFlickerTime = Time.time + flickerDelay;

        light.enabled = true;

        //light.enabled = false;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (light == null)
            return;

        if (timer > flickerDelay)
        {
            light.enabled = !light.enabled;

        }
            // Pop off an item if too big
            //while (smoothQueue.Count >= smoothing)
            //{
            //    lastSum -= smoothQueue.Dequeue();
            //}

            //float newVal = Random.Range(minIntensity, maxIntensity);
            //smoothQueue.Enqueue(newVal);
            //lastSum += newVal;

            //light.intensity = lastSum / (float)smoothQueue.Count;
        
        //if (Time.time >= nextFlickerTime)
        //{
        //    // Pop off an item if too big
        //    while (smoothQueue.Count >= smoothing)
        //    {
        //        lastSum -= smoothQueue.Dequeue();
        //    }
        //}
        //while (smoothQueue.Count >= smoothing)
        //lastSum -= smoothQueue.Dequeue();

        //float newVal = Random.Range(minIntensity, maxIntensity);
        //smoothQueue.Enqueue(newVal);
        //lastSum += newVal;

        //light.intensity = lastSum / (float)smoothQueue.Count;
        flickerDelay = Random.Range(0f, 1f);
        timer = 0;
    }
}
