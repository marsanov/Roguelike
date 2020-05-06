using System.Collections;
using System.Collections.Generic;
using Saving;
using UnityEngine;

public class CharacterTracker : MonoBehaviour, ISaveable {
    public static CharacterTracker instance;
    public int currentHealth, maxHealth, currentCoins;
    public List<GameObject> availableGuns = new List<GameObject> ();

    private void Awake () {
        if (CharacterTracker.instance == null) instance = this;
    }

    private void Start () {
        SavingWrapper.instance.Load ();
    }

    public object CaptureState () {
        Dictionary<string, object> data = new Dictionary<string, object> ();
        currentCoins = LevelManager.instance.currentCoins;
        currentHealth = PlayerHealthController.instance.currentHealth;
        maxHealth = PlayerHealthController.instance.maxHealth;

        data["coins"] = currentCoins;
        data["health"] = currentHealth;
        data["maxHealth"] = maxHealth;
        
        return data;
    }

    public void RestoreState (object state) {
        Dictionary<string, object> data = (Dictionary<string, object>) state;

        currentHealth = (int) data["health"];
        maxHealth = (int) data["maxHealth"];
        currentCoins = (int) data["coins"];

        LevelManager.instance.currentCoins = currentCoins;
        LevelManager.instance.coinTextRefresh ();

        PlayerHealthController.instance.currentHealth = currentHealth;
        PlayerHealthController.instance.maxHealth = maxHealth;
        PlayerHealthController.instance.FillHealthBar ();
    }
}