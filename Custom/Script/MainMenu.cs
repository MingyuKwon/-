using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public enum GameModeType
{
    None,
    tutorial,
    adventure1,
    adventure2,
    adventure3,

}

public class MainMenu : MonoBehaviour
{
    public class RestartManageClass
    {
        public static GameModeType restartGameModeType = GameModeType.None;
    }

    public string loadTutorialSceneName;
    public string loadAdventureSceneName;

    private void Start() {
        if(RestartManageClass.restartGameModeType != GameModeType.None)
        {
            switch(RestartManageClass.restartGameModeType)
            {
                case GameModeType.tutorial :
                    StartTutorial();
                    break;
                case GameModeType.adventure1 :
                    StartAdventure();
                    break;
            }
            RestartManageClass.restartGameModeType = GameModeType.None;
        }else
        {
            MakeScreenBlack.Clear();
            StageInformationManager.gameMode = GameModeType.None;
            GameAudioManager.instance.PlayBackGroundMusic(BackGroundAudioType.MainMenu);
            StageInformationManager.currentStageIndex = 0;
        }
        
    }

    public void StartAdventure()
    {
        MakeScreenBlack.Hide();
        LoadingInformation.loadingSceneName = loadAdventureSceneName;
        StageInformationManager.gameMode = GameModeType.adventure1;
        SceneManager.LoadScene("Before Enter Dungeon");
    }

    public void StartTutorial()
    {
        MakeScreenBlack.Hide();
        LoadingInformation.loadingSceneName = loadTutorialSceneName;
        StageInformationManager.gameMode = GameModeType.tutorial;
        SceneManager.LoadScene("Before Enter Dungeon");
    }

    public void ExitGame()
    {
        // 에디터에서 작업할 때는 이 코드가 게임이 종료되는 것처럼 작동하게 합니다.
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // 실제 빌드된 게임에서는 게임을 종료합니다.
        Application.Quit();
        #endif
    }
}
