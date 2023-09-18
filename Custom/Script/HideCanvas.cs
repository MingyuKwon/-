using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HideCanvas : MonoBehaviour
{
    public Text loadingText;
    public Slider loadingBar;
    private void Awake() {
        StartCoroutine(LoadingText());

        LoadScene(LoadingInformation.loadingSceneName);
        LoadingInformation.loadingSceneName = null;
    }

    IEnumerator LoadingText()
    {
        while(true)
        {
            loadingText.text = "Loading";
            yield return new WaitForSecondsRealtime(0.1f);

            loadingText.text = "Loading .";
            yield return new WaitForSecondsRealtime(0.1f);

            loadingText.text = "Loading . .";
            yield return new WaitForSecondsRealtime(0.1f);

            loadingText.text = "Loading . . .";
            yield return new WaitForSecondsRealtime(0.1f);
        }
        
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        yield return new WaitForSeconds(0.3f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            loadingBar.value = asyncLoad.progress;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
        
    }

}
