using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public static UIController instance;

    public Image healthSlider;
    public Text healthText, coinText;
    public GameObject deathScreen;

    [SerializeField] Image fadeScreen;
    public float fadeSpeed = 1f;
    public bool fadeIn, fadeOut;
    public Image loadingScreen;
    [SerializeField] string newGameScene;
    [SerializeField] string mainMenuScene;
    public GameObject pauseMenu;

    public Image currentGun;
    public Text gunText;

    private void Awake () {
        if (UIController.instance == null) UIController.instance = this;
    }

    IEnumerator ClosingShop (float waitTime) {
        yield return new WaitForSeconds (waitTime);
        CloseShop ();
        loadingScreen.gameObject.SetActive (false);
    }

    private void Start () {
        fadeOut = true;
        fadeIn = false;
        fadeScreen.color = new Color (fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, 1f);

        StartCoroutine (ClosingShop (0.2f));
    }

    private void Update () {
        if (fadeOut) {
            fadeScreen.color = new Color (fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards (fadeScreen.color.a, 0, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a <= 0)
                fadeOut = false;
        }

        if (fadeIn) {
            fadeScreen.color = new Color (fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards (fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a <= 0)
                fadeOut = false;
        }
    }

    public void StartFadeIn () {
        fadeIn = true;
        fadeOut = false;
    }

    public void StartNewGame () {
        Time.timeScale = 1f;
        loadingScreen.gameObject.SetActive (true);
        SceneManager.LoadScene (newGameScene);
    }

    public void GoToMainMenu () {
        Time.timeScale = 1f;
        loadingScreen.gameObject.SetActive (true);
        SceneManager.LoadScene (mainMenuScene);
    }

    public void Resume () {
        LevelManager.instance.PauseUnpause ();
    }

    public void OpenShop () {
        LevelManager.instance.isPaused = true;
        ShopController.instance.shopUI.SetActive (true);
        ShopController.instance.BalanceTextUpdate ();
    }

    public void CloseShop () {
        LevelManager.instance.isPaused = false;
        ShopController.instance.shopUI.SetActive (false);
    }
}