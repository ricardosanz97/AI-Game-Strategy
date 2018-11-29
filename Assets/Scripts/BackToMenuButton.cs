using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenuButton : MonoBehaviour {

    public void OnBackToMenu()
    {
        StartCoroutine(LoadSceneAsync(0));
    }

    private IEnumerator LoadSceneAsync(int buildIndex)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);

        while (!async.isDone)
        {
            yield return null;
        }

        AsyncOperation async2 = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        while (!async2.isDone)
            yield return null;
        
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(buildIndex));
    }
}
