using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class NewGameButtonController : MonoBehaviour
{
    public Slider Slider;
    public GameObject LoadingScreen;
    public GameObject MainMenuScreen;
    public GameObject CreditsScreen;

    public void OnNewGameButtonPressed(int buildIndex)
    {
        LoadingScreen.SetActive(true);
        MainMenuScreen.SetActive(false);
        StartCoroutine(LoadGameSceneAsync(buildIndex));
        FindObjectOfType<SoundManager>().PlayBackground(FindObjectOfType<SoundManager>().gameBackgroundSound, FindObjectOfType<SoundManager>().defaultVolume);
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }

    public void OnCreditsButtonPressed()
    {
        MainMenuScreen.SetActive(false);
        CreditsScreen.SetActive(true);
    }

    public void OnBackButtonPressed()
    {
        MainMenuScreen.SetActive(true);
        CreditsScreen.SetActive(false);
    }

    private IEnumerator LoadGameSceneAsync(int buildIndex)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);

        while (!async.isDone)
        {
            float barProgress = Mathf.Clamp01(async.progress/ 0.9f);
            Slider.value = barProgress;
            yield return null;
        }

        AsyncOperation async2 = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        while (!async2.isDone)
            yield return null;
        
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(buildIndex));
    }
}
