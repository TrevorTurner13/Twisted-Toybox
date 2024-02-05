using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private AudioClip[] buttonSounds;

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
        SettingsMenu.SetActive(false);

        SoundFXManager.instance.PlayRandomSoundFXClip(buttonSounds, transform, 1f);

    }

    public void UnpauseGame()
    {

        IsPaused = false;
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);

        SoundFXManager.instance.PlayRandomSoundFXClip(buttonSounds, transform, 1f);

    }

    public void OpenSettingsMenu()
    {
        PauseMenu.SetActive(false);
        SettingsMenu.SetActive(true);

        SoundFXManager.instance.PlayRandomSoundFXClip(buttonSounds, transform, 1f);
    }

    public void Quit()
    {
        Application.Quit();

        SoundFXManager.instance.PlayRandomSoundFXClip(buttonSounds, transform, 1f);
    }
}
