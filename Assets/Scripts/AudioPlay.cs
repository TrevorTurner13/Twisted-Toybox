using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlay : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip[] clips;
   
    public void PlaySource()
    {
        source.clip = clips[Random.Range(0, clips.Length)];
        source.Play();
    }
}
