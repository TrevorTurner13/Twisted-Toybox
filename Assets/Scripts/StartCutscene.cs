using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Cinemachine;

public class StartCutscene : MonoBehaviour
{
    public static bool isCutsceneOn;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator camAnimator;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private AudioSource cutsceneAudio;
    [SerializeField] private AudioSource levelAudio;
    [SerializeField] private AudioClip cutsceneClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.CurrentStance = PlayerMovement.playerStance.cutscene;
            SetCutsceneAudio(cutsceneClip);
            cutsceneAudio.loop = false;
            levelAudio.Pause();
            cutsceneAudio.Play();
            
            if (CompareTag("Scene1Collider"))
            {
                isCutsceneOn = true;
                camAnimator.SetBool("CutScene1", true);
                animator.SetTrigger("FirstAnimationTrigger");              
                Invoke(nameof(StopCutscene), 12f);
            }
            if (CompareTag("Scene4Collider"))
            {
                player.GetComponent<PlayerFearController>().CurrentFear = 0f;
                isCutsceneOn = true;
                camAnimator.SetBool("CutScene4", true);;
                animator.SetTrigger("FourthAnimationTrigger");
                Invoke(nameof(StopCutscene), 7f);
                
            }
        }
    }

    void StopCutscene()
    {
        if (CompareTag("Scene1Collider"))
        {
            player.CurrentStance = PlayerMovement.playerStance.standing;
            isCutsceneOn = false;
            camAnimator.SetBool("CutScene1", false);
            Destroy(gameObject);
        }
        if (CompareTag("Scene4Collider"))
        {
            player.CurrentStance = PlayerMovement.playerStance.chased;
            isCutsceneOn = false;
            camAnimator.SetBool("CutScene4", false);      
            Destroy(gameObject);
            
        }
        levelAudio.Play();
    }

    void SetBossAnimation()
    {
        animator.SetTrigger("ChaseTrigger");
    }

    void SetCutsceneAudio(AudioClip clip)
    {
        cutsceneAudio.clip = clip;
    }
}
