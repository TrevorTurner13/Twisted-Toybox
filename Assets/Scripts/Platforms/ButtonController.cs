using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour
{
    public UnityEvent onPressed;

    public void Interact()
    {
        onPressed.Invoke();
    }
}
