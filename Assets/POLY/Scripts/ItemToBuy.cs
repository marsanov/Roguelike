using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemToBuy : MonoBehaviour {
    [SerializeField] int price;
    public Text priceText;
    public ItemType type;

    //Weapon
    public GameObject weapon;
    public Transform playerWeapon;
    bool isBuyed;
    public Text buyButtonText;

    //Health
    public int restoreHp;

    private void Start () {
        if (isBuyed) {
            BuyButtonTextChange ();
        }

        priceText.text = price.ToString ();
    }

    public void OnBuy () {
        if (price <= LevelManager.instance.currentCoins) {
            if (type == ItemType.weapon && !isBuyed) {
                weapon = Instantiate (weapon, playerWeapon.position, playerWeapon.rotation);
                weapon.transform.parent = playerWeapon.transform;
                PlayerController.instance.availableGuns.Add (weapon.GetComponent<Gun> ());
                isBuyed = true;
                weapon.SetActive (false);
                BuyButtonTextChange ();
            } else if (type == ItemType.health) {

            }
            LevelManager.instance.currentCoins -= price;
            Shop.instance.BalanceTextUpdate();
        }
    }

    private void BuyButtonTextChange () {
        buyButtonText.text = "Buyed ✔️";
        buyButtonText.color = Color.green;
    }
}

public enum ItemType {
    weapon,
    health,
    ability
}