using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public string loadTutorialSceneName;
    public string loadAdventureSceneName;

    private void Start() {
        MakeScreenBlack.Clear();
        GameAudioManager.instance.PlayBackGroundMusic(BackGroundAudioType.MainMenu);
        StageInformationManager.currentStageIndex = 0;
    }

    public void StartAdventure()
    {
        MakeScreenBlack.Hide();
        LoadingInformation.loadingSceneName = loadAdventureSceneName;
        SceneManager.LoadScene("Before Enter Dungeon");
    }

    public void StartTutorial()
    {
        MakeScreenBlack.Hide();
        LoadingInformation.loadingSceneName = loadTutorialSceneName;
        SceneManager.LoadScene("Before Enter Dungeon");
    }
}
