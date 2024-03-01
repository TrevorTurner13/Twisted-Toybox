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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.CurrentStance = PlayerMovement.playerStance.cutscene;
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
    }

    void SetBossAnimation()
    {
        animator.SetTrigger("ChaseTrigger");
    }
}
