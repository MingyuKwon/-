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
        if(StageInformationManager.isnextStageDungeon && 
        LoadingInformation.loadingSceneName != "Main Menu" && 
        LoadingInformation.loadingSceneName != "Tutorial 1"&& 
        LoadingInformation.loadingSceneName != "Tutorial 2"&& 
        LoadingInformation.loadingSceneName != "Tutorial 3"&& 
        LoadingInformation.loadingSceneName != "Tutorial 4"&& 
        LoadingInformation.loadingSceneName != "Tutorial Last"
        )
        {
            loadingBar.gameObject.SetActive(true);
            PlayerSaveManager.instance.SavePlayerStageData();
        }else
        {
            loadingBar.gameObject.SetActive(false);
        }

        loadingBar.value = 0;
        yield return new WaitForSeconds(0.05f);
        loadingBar.value = 0.25f;
        yield return new WaitForSeconds(0.05f);
        loadingBar.value = 0.5f;
        yield return new WaitForSeconds(0.05f);
        loadingBar.value = 0.75f;
        yield return new WaitForSeconds(0.05f);
        loadingBar.value = 1f;

        SceneManager.LoadScene(sceneName);
        
    }

}
