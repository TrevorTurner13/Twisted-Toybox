using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour
{
    public UnityEvent onPressed;
    private bool activated = false;
    [SerializeField] private ParticleHandler particleHandler;
    public bool Activated { get { return activated; } }

    private void Start()
    {
        particleHandler = GetComponentInChildren<ParticleHandler>();
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

    
}
