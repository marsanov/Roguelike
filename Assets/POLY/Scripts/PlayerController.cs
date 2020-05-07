using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public static PlayerController instance;
    [HideInInspector] public bool canMove;
    public SpriteRenderer playerBodySR;
    //public List<Gun> availableGuns = new List<Gun> ();
    public Transform playerWeapon;
    [HideInInspector] public float dashCounter;
    [SerializeField] float moveSpeed;
    [SerializeField] float dashSpeed, dashLength, dashCooldown, dashInvincibility;
    [SerializeField] GameObject dashImpact;
    [SerializeField] int dashSoundIndex;
    // [SerializeField] float timeBetweenShots = 0.2f;
    // [SerializeField] Transform playerTransform;
    [SerializeField] FloatingJoystick movingJoystick;
    [SerializeField] FloatingJoystick shootingJoystick;
    [HideInInspector] public Vector3 aimDirection;
    [HideInInspector] public bool isShooting;

    Rigidbody2D rigidbody;
    Vector2 moveInput = new Vector2 ();
    Camera camera;
    Animator animator;
    // float shotCounter = 0;
    float activeMoveSpeed;
    float dashCooldownCounter;
    private int currentGun;

    private void Awake () {
        instance = this;
    }
    // Start is called before the first frame update
    void Start () {
        rigidbody = GetComponent<Rigidbody2D> ();
        camera = Camera.main;
        animator = GetComponent<Animator> ();
        activeMoveSpeed = moveSpeed;
        canMove = true;
        UiCurrentGunUpdate ();
    }

    // Update is called once per frame
    void Update () {
        if (canMove && !LevelManager.instance.isPaused) {
            moveInput = Vector3.up * movingJoystick.Vertical + Vector3.right * movingJoystick.Horizontal;

            rigidbody.velocity = moveInput * activeMoveSpeed;

            LookAtAim ();

            if (Input.GetKeyDown (KeyCode.Tab)) {
                SwitchGun ();
            }

            //Dash
            if (dashCounter > 0) {
                dashCounter -= Time.deltaTime;
                if (dashCounter <= 0) {
                    activeMoveSpeed = moveSpeed;
                    dashCooldownCounter = dashCooldown;
                    dashImpact.SetActive (false);
                }
            }
            if (dashCooldownCounter > 0) {
                dashCooldownCounter -= Time.deltaTime;
            }

            //Анимация ходьбы
            if (moveInput != Vector2.zero) {
                animator.SetBool ("isMoving", true);
            } else {
                animator.SetBool ("isMoving", false);
            }
        } else {
            rigidbody.velocity = Vector3.zero;
            animator.SetBool ("isMoving", false);
        }
    }

    public void Dash () {
        // if (Input.GetKeyDown (KeyCode.LeftShift)) {
        if (dashCooldownCounter <= 0 && dashCounter <= 0) {
            AudioManager.instance.PlaySFX (dashSoundIndex);
            activeMoveSpeed = dashSpeed;
            dashCounter = dashLength;
            dashImpact.SetActive (true);
            PlayerHealthController.instance.MakeInvincible (dashInvincibility);
        }
        // }
    }

    public void SwitchGun (bool shouldUpdateGunNumber = true) {
        if (shouldUpdateGunNumber && CharacterTracker.instance.availableGuns.Count > 0) {
            currentGun++;

            if (currentGun >= CharacterTracker.instance.availableGuns.Count) {
                currentGun = 0;
            }
        } else {
            Debug.Log ("Player has no guns!");
        }

        foreach (GameObject theGun in CharacterTracker.instance.availableGuns) {
            theGun.gameObject.SetActive (false);
        }
        CharacterTracker.instance.availableGuns[currentGun].gameObject.SetActive (true);

        UiCurrentGunUpdate ();
    }

    private void UiCurrentGunUpdate () {
        UIController.instance.currentGun.sprite = CharacterTracker.instance.availableGuns[currentGun].GetComponent<Gun> ().weaponSpriteRenderer.GetComponent<SpriteRenderer> ().sprite;
        UIController.instance.currentGun.SetNativeSize ();
        UIController.instance.gunText.text = CharacterTracker.instance.availableGuns[currentGun].GetComponent<Gun> ().weaponName;
    }

    private void LookAtAim () {
        aimDirection = transform.position + Vector3.up * shootingJoystick.Vertical + Vector3.right * shootingJoystick.Horizontal;
        if (aimDirection == transform.position) {
            isShooting = false;
        } else {
            Vector3 lookDirection = new Vector3 (aimDirection.x - transform.position.x, aimDirection.y - transform.position.y, 0);
            var angle = Mathf.Atan2 (lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
            isShooting = true;
        }
    }
}