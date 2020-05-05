using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
    public static Shop instance;

    [SerializeField] List<GameObject> shopCategories = new List<GameObject> ();
    [SerializeField] Text currentBalance;
    int currentShopCategory;

    private void Awake () {
        if (Shop.instance == null) Shop.instance = this;
    }

    // Start is called before the first frame update
    void Start () {
        
    }

    // Update is called once per frame
    void Update () {

    }

    public void SwitchCategory (int categoryNumber) {
        currentShopCategory = categoryNumber;

        foreach (GameObject category in shopCategories) {
            category.SetActive (false);
        }

        shopCategories[currentShopCategory].SetActive (true);
    }

    public void EnableShop () {
        gameObject.SetActive(true);
        BalanceTextUpdate();
    }

    public void DisableShop(){
        gameObject.SetActive(false);
    }

    public void BalanceTextUpdate () {
        currentBalance.text = LevelManager.instance.currentCoins.ToString ();
    }
}