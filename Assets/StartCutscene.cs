using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StartCutscene : MonoBehaviour
{
    public static bool isCutsceneOn;
    [SerializeField] private Animator jackAnimator;
    [SerializeField] private Animator camAnimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCutsceneOn = true;
            camAnimator.SetBool("CutScene1", true);
            jackAnimator.SetTrigger("FirstAnimationTrigger");
            Invoke(nameof(StopCutscene), 12f);
        }
    }

    void StopCutscene()
    {
        isCutsceneOn = false;
        camAnimator.SetBool("CutScene1", false);
    }
}
