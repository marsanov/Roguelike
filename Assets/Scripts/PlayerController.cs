using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [HideInInspector] public bool canMove;
    public SpriteRenderer playerBodySR;
    [HideInInspector] public float dashCounter;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float dashSpeed = 15f, dashLength = 1f, dashCooldown = 1f, dashInvincibility = 1f;
    [SerializeField] GameObject dashImpact;
    [SerializeField] int dashSoundIndex;
    [SerializeField] float timeBetweenShots = 0.2f;
    [SerializeField] Transform gunArm;
    [SerializeField] GameObject bulletToFire;
    [SerializeField] Transform firePoint;

    Rigidbody2D rigidbody;
    Vector2 moveInput = new Vector2();
    Camera camera;
    Animator animator;
    float shotCounter = 0;
    float activeMoveSpeed;
    float dashCooldownCounter;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        camera = Camera.main;
        animator = GetComponent<Animator>();
        activeMoveSpeed = moveSpeed;
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && !LevelManager.instance.isPaused)
        {
            moveInput.x = Input.GetAxis("Horizontal");
            moveInput.y = Input.GetAxis("Vertical");

            moveInput.Normalize();

            rigidbody.velocity = moveInput * activeMoveSpeed;

            Vector3 mousePosition = Input.mousePosition;
            Vector3 screenPoint = camera.WorldToScreenPoint(transform.localPosition);

            /*
            if (mousePosition.x < screenPoint.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                gunArm.localScale = new Vector3(-1, -1, 1);
            }
            else
            {
                transform.localScale = Vector3.one;
                gunArm.localScale = Vector3.one;
            }
            */

            //Поворот оружия
            Vector2 offset = new Vector2(mousePosition.x - screenPoint.x, mousePosition.y - screenPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            //Стрельба
            if (Input.GetMouseButton(0))
            {
                shotCounter -= Time.deltaTime;

                if (shotCounter <= 0)
                {
                    Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                    shotCounter = timeBetweenShots;
                }
            }

            //Dash
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (dashCooldownCounter <= 0 && dashCounter <= 0)
                {
                    AudioManager.instance.PlaySFX(dashSoundIndex);
                    activeMoveSpeed = dashSpeed;
                    dashCounter = dashLength;
                    dashImpact.SetActive(true);
                    PlayerHealthController.instance.MakeInvincible(dashInvincibility);
                }
            }
            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                if (dashCounter <= 0)
                {
                    activeMoveSpeed = moveSpeed;
                    dashCooldownCounter = dashCooldown;
                    dashImpact.SetActive(false);
                }
            }
            if (dashCooldownCounter > 0)
            {
                dashCooldownCounter -= Time.deltaTime;
            }

            //Анимация ходьбы
            if (moveInput != Vector2.zero)
            {
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
        else
        {
            rigidbody.velocity = Vector2.zero;
            animator.SetBool("isMoving", false);
        }
    }
}
