using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI; // Required for working with UI


public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioClip[] startGameSounds;
    [SerializeField] private AudioClip[] buttonSounds;
    public void PlayGame()
    {
        //for single sound clip
        //SoundFXManager.instance.PlaySoundFXClip(startGameSound, transform, 1f);

        //for random sound clip
        SoundFXManager.instance.PlayRandomSoundFXClip(startGameSounds, transform, 1f);

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
        SoundFXManager.instance.PlayRandomSoundFXClip(buttonSounds, transform, 1f);

        Application.Quit();
    }

    public void ButtonClicked()
    {
        SoundFXManager.instance.PlayRandomSoundFXClip(buttonSounds, transform, 1f);
    }

}
