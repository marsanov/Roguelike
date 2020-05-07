using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
    [SerializeField] GameObject mapLayout;

    [HideInInspector] public bool roomActive;

    private void Update () {

    }

    private void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "Player") {
            roomActive = true;
            if (mapLayout != null) {
                mapLayout.SetActive (true);
            }
        }
    }

    private void OnTriggerExit2D (Collider2D other) {
        if (other.tag == "Player") {
            roomActive = false;
        }
    }
}