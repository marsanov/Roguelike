using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    [Header ("Stats")]
    public int health = 150;
    [SerializeField] float combatSpeed;
    float moveSpeed;

    [Header ("Drop")]
    [SerializeField] bool shouldDropItems;
    [SerializeField] GameObject[] itemsToDrop;
    [SerializeField] float dropPercentChance;

    [Header ("Shooting")]
    [SerializeField] bool shouldShoot;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate;
    [SerializeField] float shootRange = 7f;
    [SerializeField] int shootSoundIndex;
    float fireCounter;

    [Header ("Agro")]
    [SerializeField] bool shouldChasePlayer;
    [SerializeField] float chaseDistance = 7f;
    bool inCombat;

    // [Header("Run away")]
    // [SerializeField] bool shouldRunAway;
    // [SerializeField] float rangeToChasePlayer;

    // [Header("Wandering")]
    // [SerializeField] bool shouldWander;
    // [SerializeField] float wanderLength, pauseLength;
    // private float wanderCounter, pauseCounter;
    // private Vector3 wanderDirectiion;

    [Header ("Patrolling")]
    [SerializeField] bool shouldPatrol;
    [SerializeField] float patrolSpeed;
    [SerializeField] Transform[] patrolPoints;
    int currentPatrolPoint = 0;

    [Header ("Hurt & Death")]
    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject[] deathSplatters;
    [SerializeField] int dieSoundIndex;
    [SerializeField] int hurtSoundIndex;
    [SerializeField] SpriteRenderer bodySprite;
    Vector3 moveDirection;
    Animator animator;
    Rigidbody2D rigidbody;

    [Header ("Pathfinding")]
    [SerializeField] float nextWaypointDistance = 3f;
    Path currentPath;
    int currentPathWaypoint;
    Transform pathTarget;
    Seeker seeker;

    void Start () {
        rigidbody = GetComponent<Rigidbody2D> ();
        animator = GetComponent<Animator> ();
        seeker = GetComponent<Seeker> ();
        pathTarget = this.transform;

        InvokeRepeating ("UpdatePath", 0f, .5f);

        if (shouldPatrol) moveSpeed = patrolSpeed;
    }

    void Update () {
        if (bodySprite.isVisible && PlayerController.instance.gameObject.activeInHierarchy) {
            Move ();
            Shoot ();
        } else {
            rigidbody.velocity = Vector3.zero;
        }
    }

    private void Shoot () {
        if (shouldShoot && Vector3.Distance (transform.position, PlayerController.instance.transform.position) < shootRange) {
            if (!isPlayerVisible ()) return;

            //Поворот за игроком
            LookAt (PlayerController.instance.transform);

            fireCounter -= Time.deltaTime;
            if (fireCounter <= 0) {
                fireCounter = fireRate;
                Instantiate (bullet, firePoint.position, firePoint.rotation);
                AudioManager.instance.PlaySFX (shootSoundIndex);
            }
        }
    }

    bool isPlayerVisible () {
        LayerMask checkLayer = LayerMask.GetMask ("Obstacle");
        RaycastHit2D hit = Physics2D.Raycast (transform.position, PlayerController.instance.transform.position - transform.position, shootRange, checkLayer);
        Debug.DrawRay (transform.position, PlayerController.instance.transform.position - transform.position, Color.red);
        if (hit.collider == null) {
            return true;
        } else {
            return false;
        }
    }

    private void LookAt (Transform target) {
        Vector2 rotateDirection = new Vector2 (target.position.x - transform.position.x, target.position.y - transform.position.y);
        var angle = Mathf.Atan2 (rotateDirection.y, rotateDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
    }

    void OnPathComplete (Path p) {
        if (!p.error) {
            currentPath = p;
            currentPathWaypoint = 0;
        }
    }

    void UpdatePath () {
        seeker.StartPath (transform.position, pathTarget.position, OnPathComplete);
    }

    private void Move () {
        pathTarget = this.transform;
        moveDirection = Vector3.zero;
        float distanceToPlayer = Vector3.Distance (transform.position, PlayerController.instance.transform.position);

        if (inCombat || (distanceToPlayer < chaseDistance && shouldChasePlayer && distanceToPlayer > 2f)) {
            if (!inCombat && isPlayerVisible ()) {
                inCombat = true;
                moveSpeed = combatSpeed;
            }
            if (inCombat) {
                pathTarget = PlayerController.instance.transform;
                if (isPlayerVisible ()) {
                    LookAt (PlayerController.instance.transform);
                } else {
                    //Look forward to path
                    Vector2 rotateDirection = new Vector2 (currentPath.vectorPath[currentPathWaypoint].x - transform.position.x, currentPath.vectorPath[currentPathWaypoint].y - transform.position.y);
                    var angle = Mathf.Atan2 (rotateDirection.y, rotateDirection.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
                }
            }
        } else if (shouldPatrol) {
            if (Vector3.Distance (transform.position, patrolPoints[currentPatrolPoint].position) < .2f) {
                currentPatrolPoint++;
                if (currentPatrolPoint >= patrolPoints.Length) {
                    currentPatrolPoint = 0;
                }
            }

            pathTarget = patrolPoints[currentPatrolPoint];
        }

        if (currentPath == null) return;
        if (currentPathWaypoint >= currentPath.vectorPath.Count - 1) return;
        if (Vector3.Distance (transform.position, currentPath.vectorPath[currentPathWaypoint]) <= nextWaypointDistance) currentPathWaypoint++;

        moveDirection = currentPath.vectorPath[currentPathWaypoint] - transform.position;
        moveDirection.Normalize ();

        rigidbody.velocity = moveDirection * moveSpeed;

        if (moveDirection != Vector3.zero) {
            animator.SetBool ("isMoving", true);
        } else {
            animator.SetBool ("isMoving", false);
        }
    }

    public void DamaGeEnemy (int damage) {
        health -= damage;
        AudioManager.instance.PlaySFX (hurtSoundIndex);
        Instantiate (hitEffect, transform.position, transform.rotation);
        if (health <= 0) {
            DropItems ();
            Destroy (gameObject);
            AudioManager.instance.PlaySFX (dieSoundIndex);
            int selectedSplatter = Random.Range (0, deathSplatters.Length - 1);
            int rotation = Random.Range (0, 4);
            Instantiate (deathSplatters[selectedSplatter], transform.position, Quaternion.Euler (0, 0, rotation * 90f));
        }
    }

    void DropItems () {
        //Дроп лута
        if (shouldDropItems) {
            float dropChance = Random.Range (0f, 100f);
            if (dropChance < dropPercentChance) {
                int randomItem = Random.Range (0, itemsToDrop.Length);
                Instantiate (itemsToDrop[randomItem], transform.position, transform.rotation);
            }
        }
    }

    private void OnDrawGizmos () {
        if (shouldShoot) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere (transform.position, shootRange);
        }
        if (shouldChasePlayer) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere (transform.position, chaseDistance);
        }
    }
}