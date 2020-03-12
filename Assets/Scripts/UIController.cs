using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Image healthSlider;
    public Text healthText, coinText;
    public GameObject deathScreen;

    [SerializeField] Image fadeScreen;
    public float fadeSpeed = 1f;
    public bool fadeIn, fadeOut;
    [SerializeField] string newGameScene;
    [SerializeField] string mainMenuScene;
    public GameObject pauseMenu;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        fadeOut = true;
        fadeIn = false;
        fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, 1f);
    }

    private void Update()
    {
        if (fadeOut)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a <= 0)
                fadeOut = false;
        }

        if (fadeIn)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a <= 0)
                fadeOut = false;
        }
    }

    public void StartFadeIn()
    {
        fadeIn = true;
        fadeOut = false;
    }

    public void StartNewGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(newGameScene);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void Resume()
    {
        LevelManager.instance.PauseUnpause();
    }
}
