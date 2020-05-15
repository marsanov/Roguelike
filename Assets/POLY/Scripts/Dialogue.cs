using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {
    [SerializeField] List<string> dialogueReplics = new List<string> ();
    [SerializeField] GameObject interactButtonPlaceholder, dialogueScreen;
    [SerializeField] Text dialogueText;
    [SerializeField] bool shouldBeginNewLevel;

    //Shop
    [SerializeField] ShopController shop;
    [SerializeField] bool shouldOpenShop;

    bool nearPlayer, interactWithPlayer;
    int currentDialogueTextIndex = -1;
    bool dialogueComplete;
    bool endDialogue;

    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        if (nearPlayer && interactWithPlayer && !dialogueComplete) {
            dialogueScreen.SetActive (true);

            dialogueText.text = dialogueReplics[currentDialogueTextIndex];
        }
    }

    public void Talk () {
        // if (Input.GetKeyDown (KeyCode.E) && nearPlayer) {
        interactWithPlayer = true;
        // currentDialogueTextIndex++;
        if (!shouldOpenShop) currentDialogueTextIndex = Random.Range (0, dialogueReplics.Count);
        else endDialogue = true;

        if (endDialogue) {
            EndDialogue ();

            if (shouldOpenShop) {
                UIController.instance.OpenShop ();
            }

            if (shouldBeginNewLevel) {
                LevelManager.instance.StartCoroutine ("LevelEnd");
                dialogueComplete = true;
            }
        }

        endDialogue = true;

        // if (currentDialogueTextIndex >= dialogueReplics.Length) {
        //     EndDialogue ();

        //     if (shouldOpenShop) {
        //         UIController.instance.OpenShop ();
        //     }

        //     if (shouldBeginNewLevel) {
        //         LevelManager.instance.StartCoroutine ("LevelEnd");
        //         dialogueComplete = true;
        //     }
        // }
        // }
    }

    private void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "Player" && !dialogueComplete) {
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
        // currentDialogueTextIndex = -1;
        endDialogue = false;
    }
}