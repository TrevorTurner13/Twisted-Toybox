using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScareTrigger : MonoBehaviour
{
    [SerializeField] private Animator jumpScareAnimator;
    [SerializeField] private string jumpScareTriggerName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SetTrigger(jumpScareTriggerName);
        Destroy(gameObject);
    }

    public void SetTrigger(string triggerName)
    {
        jumpScareAnimator.SetTrigger(triggerName);
    }
}
