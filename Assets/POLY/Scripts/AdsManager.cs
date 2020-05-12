using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener {
    public static AdsManager instance;
    string placement = "rewardedVideo";

    private void Awake () {
        if (AdsManager.instance == null) instance = this;
    }

    // Start is called before the first frame update
    void Start () {
        Advertisement.AddListener (AdsManager.instance);
        Advertisement.Initialize ("3600480", true);
    }

    public void ShowAd (string p) {
        Advertisement.Show (p);
    }

    public void OnUnityAdsDidFinish (string message, ShowResult showResult) {
        if (showResult == ShowResult.Finished) {
            //Наградить игрока за просмотр рекламы
            LevelManager.instance.GetCoins (5);
            ShopController.instance.BalanceTextUpdate ();
            SavingWrapper.instance.Save ();
        } else {
            //Ничего не получит
        }
    }

    public void OnUnityAdsDidStart (string message) {

    }

    public void OnUnityAdsReady (string message) {

    }

    public void OnUnityAdsDidError (string message) {

    }
}