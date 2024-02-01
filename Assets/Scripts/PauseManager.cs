using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public PlayerInputs inputActions;
    public static PauseManager instance;

    [SerializeField] private GameObject PauseMenu;

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        PauseMenu.SetActive(false);
    }

    public void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0f;
        PauseMenu.SetActive(true);
    }

    public void UnpauseGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
    }
}
