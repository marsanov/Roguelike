using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager instance;
    [SerializeField] AudioSource levelMusic, gameOverMusic, winMusic;
    [SerializeField] AudioSource[] sfx;

    private void Awake () {
        instance = this;
        levelMusic.Play ();
    }

    //Public methods
    public void PlayGameOver () {
        levelMusic.Stop ();
        gameOverMusic.Play ();
    }

    public void PlayLevelWin () {
        levelMusic.Stop ();
        winMusic.Play ();
    }

    public void PlaySFX (int sfxToPlay) {
        sfx[sfxToPlay].Stop ();
        sfx[sfxToPlay].Play ();
    }
}