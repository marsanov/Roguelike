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
    Animator animator;
    //float shotCounter = 0;
    float reloadCounter = 0;
    int bulletsCounter;

    // Start is called before the first frame update
    void Start () {
        animator = GetComponent<Animator> ();
        if (reloadTime == 0) reloadTime = timeBetweenShots;
    }

    // Update is called once per frame
    void Update () {
        if (PlayerController.instance.canMove && !LevelManager.instance.isPaused) {

            animator.SetBool ("isShooting", true);
            if (PlayerController.instance.isShooting) {
                if (reloadCounter > 0) {
                    reloadCounter -= Time.deltaTime;
                } else {
                    foreach (Transform firePoint in firePoints) {
                        Instantiate (bulletToFire, firePoint.position, firePoint.rotation);
                    }
                    bulletsCounter++;
                    if (bulletsCounter < bulletsPerShot) {
                        reloadCounter = timeBetweenShots;
                    } else {
                        bulletsCounter = 0;
                        reloadCounter = reloadTime;
                    }
                }
            } else {
                animator.SetBool ("isShooting", false);
                bulletsCounter = 0;
            }
        }
    }
}