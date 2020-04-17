using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Stats")]
    public int health = 150;
    [SerializeField] float moveSpeed = 5f;

    [Header("Drop")]
    [SerializeField] bool shouldDropItems;
    [SerializeField] GameObject[] itemsToDrop;
    [SerializeField] float dropPercentChance;

    [Header("Shooting")]
    [SerializeField] bool shouldShoot;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate;
    [SerializeField] float shootRange = 7f;
    [SerializeField] int shootSoundIndex;
    float fireCounter;

    [Header("Agro")]
    [SerializeField] bool shouldChasePlayer;
    [SerializeField] float chaseDistance = 7f;
    bool inCombat;

    [Header("Run away")]
    [SerializeField] bool shouldRunAway;
    [SerializeField] float rangeToChasePlayer;

    [Header("Wandering")]
    [SerializeField] bool shouldWander;
    [SerializeField] float wanderLength, pauseLength;
    private float wanderCounter, pauseCounter;
    private Vector3 wanderDirectiion;

    [Header("Patrolling")]
    [SerializeField] bool shouldPatrol;
    [SerializeField] Transform[] patrolPoints;
    int currentPatrolPoint = 0;

    [Header("Hurt & Death")]
    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject[] deathSplatters;
    [SerializeField] int dieSoundIndex;
    [SerializeField] int hurtSoundIndex;
    [SerializeField] SpriteRenderer bodySprite;
    Vector3 moveDirection;
    Animator animator;
    Rigidbody2D rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (shouldWander)
        {
            wanderCounter = Random.Range(wanderLength * 0.75f, wanderLength * 1.25f);
            pauseCounter = Random.Range(pauseLength * 0.75f, pauseLength * 1.25f);
        }
    }

    void Update()
    {
        if (bodySprite.isVisible && PlayerController.instance.gameObject.activeInHierarchy)
        {
            Move();
            Shoot();
        }
        else
        {
            rigidbody.velocity = Vector3.zero;
        }
    }

    private void Shoot()
    {
        if (shouldShoot && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < shootRange)
        {
            //Поворот за игроком
            LookAt(PlayerController.instance.transform);

            fireCounter -= Time.deltaTime;
            if (fireCounter <= 0)
            {
                fireCounter = fireRate;
                Instantiate(bullet, firePoint.position, firePoint.rotation);
                AudioManager.instance.PlaySFX(shootSoundIndex);
            }
        }
    }

    private void LookAt(Transform target)
    {
        Vector2 rotateDirection = new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y);
        var angle = Mathf.Atan2(rotateDirection.y, rotateDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Move()
    {
        moveDirection = Vector3.zero;
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerController.instance.transform.position);

        if (distanceToPlayer < chaseDistance && shouldChasePlayer && distanceToPlayer > 2f)
        {
            moveDirection = PlayerController.instance.transform.position - transform.position;
            LookAt(PlayerController.instance.transform);
        }
        else
        {
            if (shouldWander)
            {
                if (wanderCounter > 0)
                {
                    wanderCounter -= Time.deltaTime;

                    //Move the enemy
                    moveDirection = wanderDirectiion;

                    if (wanderCounter <= 0)
                    {
                        pauseCounter = Random.Range(pauseLength * 0.75f, pauseLength * 1.25f);
                    }
                }

                if (pauseCounter > 0)
                {
                    pauseCounter -= Time.deltaTime;
                    if (pauseCounter <= 0)
                    {
                        wanderCounter = Random.Range(wanderLength * 0.75f, wanderLength * 1.25f);

                        wanderDirectiion = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                    }
                }
            }

            if (shouldPatrol)
            {
                moveDirection = patrolPoints[currentPatrolPoint].position - transform.position;

                if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < .2f)
                {
                    currentPatrolPoint++;
                    if (currentPatrolPoint >= patrolPoints.Length)
                    {
                        currentPatrolPoint = 0;
                    }
                }

                LookAt(patrolPoints[currentPatrolPoint]);
            }
        }
        if (shouldRunAway && distanceToPlayer < rangeToChasePlayer)
        {
            moveDirection = transform.position - PlayerController.instance.transform.position;
        }
        /*
        else
        {
            moveDirection = Vector3.zero;
        }
        */
        moveDirection.Normalize();

        rigidbody.velocity = moveDirection * moveSpeed;

        if (moveDirection != Vector3.zero)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    public void DamaGeEnemy(int damage)
    {
        health -= damage;
        AudioManager.instance.PlaySFX(hurtSoundIndex);
        Instantiate(hitEffect, transform.position, transform.rotation);
        if (health <= 0)
        {
            DropItems();
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(dieSoundIndex);
            int selectedSplatter = Random.Range(0, deathSplatters.Length - 1);
            int rotation = Random.Range(0, 4);
            Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0, 0, rotation * 90f));
        }
    }

    void DropItems()
    {
        //Дроп лута
        if (shouldDropItems)
        {
            float dropChance = Random.Range(0f, 100f);
            if (dropChance < dropPercentChance)
            {
                int randomItem = Random.Range(0, itemsToDrop.Length);
                Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
            }
        }
    }
}
