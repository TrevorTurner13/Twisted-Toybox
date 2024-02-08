using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    [SerializeField] private PlayerMovement player;

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

    private void Start()
    {
        player = FindAnyObjectByType<PlayerMovement>();
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

        player.IsPaused = false;

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

    public void LoadScene()
    {
        var op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        op.completed += (x) =>
        {
           Debug.Log("Loaded");
           player = FindAnyObjectByType<PlayerMovement>();
        };
    }
}
