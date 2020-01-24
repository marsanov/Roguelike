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
    
    private void Awake()
    {
        instance = this;
    }
}
