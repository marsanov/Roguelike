using System.Collections;
using System.Collections.Generic;
using Saving;
using UnityEngine;
using UnityEngine.UI;

public class ItemToBuy : MonoBehaviour, ISaveable {
    [SerializeField] int price;
    public Text priceText;
    public ItemType type;

    //Weapon
    public GameObject weapon;
    bool isBuyed;
    public Text buyButtonText;

    //Health
    public int restoreHp;
    [SerializeField] int healSoundIndex;

    private void Start () {
        priceText.text = price.ToString ();
    }

    public void OnBuy () {
        if (price <= LevelManager.instance.currentCoins) {
            if (type == ItemType.weapon && !isBuyed) {
                InstantiateWeapon ();
                isBuyed = true;
                BuyButtonTextChange ();
            } else if (type == ItemType.health) {
                PlayerHealthController.instance.HealPlayer (restoreHp);
                AudioManager.instance.PlaySFX (healSoundIndex);
            }
            LevelManager.instance.currentCoins -= price;
            LevelManager.instance.coinTextRefresh ();
            ShopController.instance.BalanceTextUpdate ();

            SavingWrapper.instance.Save ();
        }
    }

    private void InstantiateWeapon () {
        Transform playerWeapon = PlayerController.instance.playerWeapon;
        weapon = Instantiate (weapon, playerWeapon.position, playerWeapon.rotation);
        weapon.transform.parent = playerWeapon.transform;
        CharacterTracker.instance.availableGuns.Add (weapon);
        weapon.SetActive (false);
    }

    private void BuyButtonTextChange () {
        if (isBuyed) {
            buyButtonText.text = "Buyed ✔️";
            buyButtonText.color = Color.green;
        } else {
            buyButtonText.text = "Buy";
            buyButtonText.color = Color.white;
        }
    }

    public object CaptureState () {
        return isBuyed;
    }

    public void RestoreState (object state) {
        isBuyed = (bool) state;
        if (isBuyed) {
            BuyButtonTextChange ();
            InstantiateWeapon ();
        }
        UIController.instance.CloseShop ();
    }
}

public enum ItemType {
    weapon,
    health,
    ability
}