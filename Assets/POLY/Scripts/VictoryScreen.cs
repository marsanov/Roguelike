using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour {
    [SerializeField] float waitForAnyKey = 2f;
    [SerializeField] string mainMenuScene;
    [SerializeField] GameObject pressAnyKeyText;

    private void Start () {
        Time.timeScale = 1f;
    }
    private void Update () {
        if (waitForAnyKey > 0) {
            waitForAnyKey -= Time.deltaTime;
            if (waitForAnyKey <= 0) {
                pressAnyKeyText.SetActive (true);
            }
        } else {
            if (Input.anyKey) {
                // SceneManager.LoadScene(mainMenuScene);
                AsyncOperation loadingOperation = SceneManager.LoadSceneAsync (mainMenuScene);
            }
        }
    }
}