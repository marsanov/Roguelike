using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {
    [SerializeField] string[] dialogueReplics;
    [SerializeField] GameObject interactButtonPlaceholder, dialogueScreen;
    [SerializeField] Text dialogueText;
    [SerializeField] bool shouldBeginNewLevel;

    //Shop
    [SerializeField] ShopController shop;
    [SerializeField] bool shouldOpenShop;

    bool nearPlayer, interactWithPlayer;
    int currentDialogueTextIndex = -1;

    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown (KeyCode.E) && nearPlayer) {
            interactWithPlayer = true;
            currentDialogueTextIndex++;
            if (currentDialogueTextIndex >= dialogueReplics.Length) {
                EndDialogue ();

                if (shouldOpenShop) {
                    UIController.instance.OpenShop();
                }

                if (shouldBeginNewLevel) {
                    LevelManager.instance.StartCoroutine ("LevelEnd");
                }
            }
        }

        if (nearPlayer && interactWithPlayer) {
            dialogueScreen.SetActive (true);

            dialogueText.text = dialogueReplics[currentDialogueTextIndex];
        }
    }

    private void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "Player") {
            interactButtonPlaceholder.gameObject.SetActive (true);
            nearPlayer = true;
        }
    }
    private void OnTriggerExit2D (Collider2D other) {
        if (other.tag == "Player") {
            EndDialogue ();
        }
    }

    private void EndDialogue () {
        interactButtonPlaceholder.gameObject.SetActive (false);
        nearPlayer = false;
        interactWithPlayer = false;
        dialogueScreen.SetActive (false);
        currentDialogueTextIndex = -1;
    }
}