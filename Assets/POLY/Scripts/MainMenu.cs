using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    [SerializeField] string sceneToLoad;
    [SerializeField] Image loadingScreen;

    public void StartGame () {
        loadingScreen.gameObject.SetActive (true);
        // SceneManager.LoadScene (sceneToLoad);
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync (sceneToLoad);
    }

    public void ExitGame () {
        Application.Quit ();
    }
}