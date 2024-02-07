using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;

    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private AudioClip[] buttonSounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        GameOverPanel.SetActive(false);
    }

    public void GameOverPanelActive()
    {
        GameOverPanel.SetActive(true);

        Debug.Log("game over");
    }

    public void RespawnLastCheckpoint()
    {
        StartCoroutine(LoadLastCheckPointCoroutine());

    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(LoadMainMenuCoroutine());

    }

    IEnumerator LoadLastCheckPointCoroutine()
    {
        yield return new WaitForSeconds(0.5f);


        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    IEnumerator LoadMainMenuCoroutine()
    {
        yield return new WaitForSeconds(0.5f);


        SceneManager.LoadScene(1);
    }


    public void ButtonClicked()
    {
        SoundFXManager.instance.PlayRandomSoundFXClip(buttonSounds, transform, 1f);
    }
}
