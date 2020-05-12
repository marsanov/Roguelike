using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    public string weaponName;
    public SpriteRenderer weaponSpriteRenderer;
    public bool available;
    [SerializeField] float timeBetweenShots = 0.2f;
    [SerializeField] GameObject bulletToFire;
    [SerializeField] int bulletsPerShot;
    [SerializeField] float reloadTime;
    [SerializeField] Transform[] firePoints;

    [Header ("Sound")]
    [SerializeField] int shotMinIndex;
    [SerializeField] int shotMaxIndex;
    [SerializeField] int reloadMinIndex, reloadMaxIndex;

    Animator animator;
    //float shotCounter = 0;
    float reloadCounter = 0;
    int bulletsCounter;
    bool reloaded;

    // Start is called before the first frame update
    void Start () {
        animator = GetComponent<Animator> ();
        reloaded = true;
        if (reloadTime == 0) reloadTime = timeBetweenShots;
    }

    // Update is called once per frame
    void Update () {
        if (PlayerController.instance.canMove && !LevelManager.instance.isPaused) {

            if (PlayerController.instance.isShooting) {
                animator.SetBool ("isShooting", true);
                if (reloadCounter > 0) {
                    reloadCounter -= Time.deltaTime;
                } else {
                    reloaded = false;
                    foreach (Transform firePoint in firePoints) {
                        Instantiate (bulletToFire, firePoint.position, firePoint.rotation);
                        AudioManager.instance.PlaySFX (Random.Range (shotMinIndex, shotMaxIndex));
                    }
                    bulletsCounter++;
                    if (bulletsCounter < bulletsPerShot) {
                        reloadCounter = timeBetweenShots;
                    } else {
                        bulletsCounter = 0;
                        reloadCounter = reloadTime;
                        AudioManager.instance.PlaySFX (Random.Range (reloadMinIndex, reloadMaxIndex));
                        reloaded = true;
                    }
                }
            } else {
                if (!reloaded) {
                    AudioManager.instance.PlaySFX (Random.Range (reloadMinIndex, reloadMaxIndex));
                    reloaded = true;
                }

                if (animator.GetBool ("isShooting")) {
                    animator.SetBool ("isShooting", false);
                }
                bulletsCounter = 0;
            }
        }
    }
}