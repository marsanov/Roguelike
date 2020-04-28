using System.Collections;
using System.Collections.Generic;
using Saving;
using UnityEngine;

public class SavingWrapper : MonoBehaviour {
    const string defaultSaveFile = "save";
    public static SavingWrapper instance;

    private void Awake () {
        if(SavingWrapper.instance == null) instance = this;
    }

    // IEnumerator Start(){
    //     yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
    // }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown (KeyCode.F9)) {
            Load ();
        }
        if (Input.GetKeyDown (KeyCode.F5)) {
            Save ();
        }
    }

    public void Load () {
        GetComponent<SavingSystem> ().Load (defaultSaveFile);
    }

    public void Save () {
        GetComponent<SavingSystem> ().Save (defaultSaveFile);
    }
}