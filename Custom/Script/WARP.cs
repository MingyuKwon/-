using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WARP : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        SceneManager.LoadScene("Cave Rest");
        StageManager.isNowInitializing = true;
    }
}
