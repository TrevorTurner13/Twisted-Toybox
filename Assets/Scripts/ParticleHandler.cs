using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;
public class ParticleHandler : MonoBehaviour
{
    [SerializeField] private AudioClip particleSFX;

    private List<ParticleSystem> particles = new List<ParticleSystem>();

    private bool shouldPlay = true;
    public bool ShouldPlay { get { return shouldPlay; } set { shouldPlay = value; } }
    
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
        
        if (shouldPlay)
        {
            foreach (ParticleSystem particle in particles)
            {
                particle.Play();
            }
            SoundFXManager.instance.PlaySoundFXClip(particleSFX, transform, 0.1f);
        }
    }
}
