using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI; // Required for working with UI


public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioClip startGameSound;

    public void PlayGame()
    {
        SoundFXManager.instance.PlaySoundFXClip(startGameSound, transform, 1f);

        StartCoroutine(LoadSceneCoroutine());
    }

    IEnumerator LoadSceneCoroutine()
    {
        yield return new WaitForSeconds(3.25f);
        SceneManager.LoadScene(2);
    }

    //public void PlayGame()
    //{

    //    SceneManager.LoadScene(2);

    //}

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Back()
    {
        SceneManager.LoadScene(1);
    }

}
