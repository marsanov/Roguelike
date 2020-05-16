using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour {
    public static ShopController instance;

    [SerializeField] GameObject[] shopCategories;
    [SerializeField] Text currentBalance;
    public GameObject shopUI;
    int currentShopCategory;

    private void Awake () {
        if (ShopController.instance == null) ShopController.instance = this;
    }

    public void SwitchCategory (int categoryNumber) {
        currentShopCategory = categoryNumber;

        if (shopCategories != null) {
            foreach (GameObject category in shopCategories) {
                category.SetActive (false);
            }

            shopCategories[currentShopCategory].SetActive (true);
        } else {
            Debug.Log ("Shop is empty");
        }

    }

    public void BalanceTextUpdate () {
        currentBalance.text = LevelManager.instance.currentCoins.ToString ();
    }

    public void Close () {
        UIController.instance.CloseShop ();
    }
}