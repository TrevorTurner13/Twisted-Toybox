using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackAnimationScript : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject jack;
    [SerializeField] PlayerMovement player;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] GameObject falsePuppet;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.gameObject.SetActive(false);
            virtualCamera.m_Follow = falsePuppet.transform;
            animator.SetTrigger("PlayerGrabTrigger");
            
        }
    }
}
