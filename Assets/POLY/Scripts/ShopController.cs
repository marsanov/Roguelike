using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour {
    public static ShopController instance;

    [SerializeField] List<GameObject> shopCategories = new List<GameObject> ();
    [SerializeField] Text currentBalance;
    public GameObject shopUI;
    int currentShopCategory;

    private void Awake () {
        if (ShopController.instance == null) ShopController.instance = this;
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

    public void BalanceTextUpdate () {
        currentBalance.text = LevelManager.instance.currentCoins.ToString ();
    }
}