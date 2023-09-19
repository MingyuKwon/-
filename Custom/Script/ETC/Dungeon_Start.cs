using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Dungeon_Start : MonoBehaviour
{
    public string loadSceneName;

    void Start()
    {
        StageManager.StageSaveManager.isnextStageDungeon = true;

        SceneManager.LoadScene("Loading");
        LoadingInformation.loadingSceneName = loadSceneName;
    }

}
