using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;
public class ParticleHandler : MonoBehaviour
{
    [SerializeField] private AudioClip particleSFX;

    private List<ParticleSystem> particles = new List<ParticleSystem>();
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            ParticleSystem particle = transform.GetChild(i).GetComponent<ParticleSystem>();

            if (particle != null)
            {
                particles.Add(particle);
            }
        }
    }

    public void RunParticles()
    {
        foreach (ParticleSystem particle in particles)
        {
            SoundFXManager.instance.PlaySoundFXClip(particleSFX, transform, 0.1f);
            particle.Play();
        }
    }
}
