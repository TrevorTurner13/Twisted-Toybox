using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour
{
    public UnityEvent onPressed;
    private bool activated = false;
    private Animator animator;
    [SerializeField] private ParticleHandler particleHandler;
    private AudioSource switchSource;

    public bool Activated { get { return activated; } }

    private void Start()
    {
        particleHandler = GetComponentInChildren<ParticleHandler>();
        animator = GetComponent<Animator>();
        switchSource = GetComponentInChildren<AudioSource>();
    }

    public void SwitchOn()
    {
        if (animator != null)
        {
            animator.SetBool("On", !animator.GetBool("On"));
        }
        PlaySound();
        

    }

    public void Interact()
    {
        onPressed.Invoke();   
    }

    public void ActivateDeactivate()
    {
        activated = !activated;
    }

    public void RunParticles()
    {
        particleHandler.RunParticles();
    }

    public void PlaySound()
    {
        if (switchSource != null)
        {
            switchSource.Play();
        }
    }
}
