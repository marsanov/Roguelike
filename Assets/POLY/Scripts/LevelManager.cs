using System.Collections;
using System.Collections.Generic;
using Saving;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    public static LevelManager instance;
    [SerializeField] float waitToLoad = 1f;
    [SerializeField] string nextLevel;
    public bool isPaused;
    public int currentCoins;

    private void Awake () {
        instance = this;
    }

    private void Start () {
        currentCoins = CharacterTracker.instance.currentCoins;
        Time.timeScale = 1f;
        coinTextRefresh ();
    }
    private void Update () {
        if (Input.GetKeyDown (KeyCode.Escape)) {
            PauseUnpause ();
        }
    }

    //Public methods
    public IEnumerator LevelEnd () {
        yield return new WaitForSeconds (5);
        AudioManager.instance.PlayLevelWin ();
        UIController.instance.StartFadeIn ();
        PlayerController.instance.canMove = false;
        yield return new WaitForSeconds (waitToLoad);

        CharacterTracker.instance.currentCoins = currentCoins;
        CharacterTracker.instance.currentHealth = PlayerHealthController.instance.currentHealth;
        CharacterTracker.instance.maxHealth = PlayerHealthController.instance.maxHealth;

        SavingWrapper.instance.Save ();

        SceneManager.LoadScene (nextLevel);
    }

    public void PauseUnpause () {
        if (!isPaused) {
            UIController.instance.pauseMenu.SetActive (true);
            isPaused = true;
            Time.timeScale = 0f;
        } else {
            UIController.instance.pauseMenu.SetActive (false);
            isPaused = false;
            Time.timeScale = 1f;
        }
    }

    public void GetCoins (int amount) {
        currentCoins += amount;
        coinTextRefresh ();
    }

    public void SpendCoins (int amount) {
        currentCoins -= amount;

        if (currentCoins < 0) {
            currentCoins = 0;
        }

        coinTextRefresh ();
    }

    public void coinTextRefresh () {
        UIController.instance.coinText.text = currentCoins.ToString ();
    }
}