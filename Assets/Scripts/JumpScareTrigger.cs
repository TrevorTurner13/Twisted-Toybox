using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScareTrigger : MonoBehaviour
{
    [SerializeField] private Animator jumpScareAnimator;
    [SerializeField] private string jumpScareTriggerName;
    [SerializeField] private AudioSource jumpScareSource;
    [SerializeField] private AudioClip jumpScareClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SetTrigger(jumpScareTriggerName);
        SetAudioClip();
        jumpScareSource.Play();
        Destroy(gameObject);
    }

    public void SetTrigger(string triggerName)
    {
        jumpScareAnimator.SetTrigger(triggerName);
    }

    public void SetAudioClip()
    {
        jumpScareSource.clip = jumpScareClip;
    }
}
