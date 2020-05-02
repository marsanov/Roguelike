using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    [SerializeField] float timeBetweenShots = 0.2f;
    [SerializeField] GameObject bulletToFire;
    [SerializeField] Transform firePoint;
    float shotCounter = 0;
    Animator animator;

    // Start is called before the first frame update
    void Start () {
        animator = GetComponent<Animator> ();
    }

    // Update is called once per frame
    void Update () {
        if (PlayerController.instance.canMove && !LevelManager.instance.isPaused) {
            animator.SetBool ("isShooting", true);
            if (Input.GetMouseButton (0) || Input.GetMouseButtonDown (0)) {
                if (shotCounter > 0) {
                    shotCounter -= Time.deltaTime;
                } else {
                    //shotCounter -= Time.deltaTime;
                    Instantiate (bulletToFire, firePoint.position, firePoint.rotation);
                    shotCounter = timeBetweenShots;
                }
            } else {
                animator.SetBool ("isShooting", false);
            }
        }
    }
}