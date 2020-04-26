using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour {
    public static PlayerHealthController instance;

    public int currentHealth;
    public int maxHealth = 100;
    [SerializeField] float damageInvincLength = 1f;
    [SerializeField] int hurtSoundIndex;

    float invincCount;

    private void Awake () {
        instance = this;
    }
    // Start is called before the first frame update
    void Start () {
        maxHealth = CharacterTracker.instance.maxHealth;
        currentHealth = CharacterTracker.instance.currentHealth;
        //currentHealth = maxHealth;
        FillHealthBar ();
    }

    // Update is called once per frame
    void Update () {
        if (invincCount > 0) {
            invincCount -= Time.deltaTime;
            if (invincCount <= 0) {
                PlayerController.instance.playerBodySR.color = new Color (1, 1, 1, 1);
            }
        }
    }

    //Public methods
    public void DamagePlayer (int damage) {
        if (invincCount <= 0) {
            AudioManager.instance.PlaySFX (hurtSoundIndex);
            PlayerController.instance.playerBodySR.color = new Color (1, 0, 0, 0.5f);
            invincCount = damageInvincLength;
            currentHealth -= damage;
            FillHealthBar ();

            if (currentHealth <= 0) {
                PlayerController.instance.gameObject.SetActive (false);
                UIController.instance.deathScreen.SetActive (true);
                AudioManager.instance.PlayGameOver ();
            }
        }
    }

    public void MakeInvincible (float length) {
        invincCount = length;
        Color playerColor = PlayerController.instance.playerBodySR.color;
        PlayerController.instance.playerBodySR.color = new Color (playerColor.r, playerColor.g, playerColor.b, 0.5f);
    }
    public void HealPlayer (int healAmount) {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        FillHealthBar ();
    }

    //Private methods
    public void FillHealthBar () {
        UIController.instance.healthSlider.fillAmount = (float) currentHealth / maxHealth;
        UIController.instance.healthText.text = currentHealth + " / " + maxHealth;
    }
}