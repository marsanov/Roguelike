using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Image healthSlider;
    public Text healthText;
    public GameObject deathScreen;

    [SerializeField] Image fadeScreen;
    public float fadeSpeed = 1f;
    public bool fadeIn, fadeOut;

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
}
