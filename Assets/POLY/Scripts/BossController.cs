using System.Collections;
using Pathfinding;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossController : MonoBehaviour {
    public static BossController instance;
    [SerializeField] BossAction[] actions;
    [SerializeField] Rigidbody2D rigidbody;
    [SerializeField] int currentBossHealth;
    [SerializeField] GameObject deathEffect;
    [SerializeField] GameObject bossCanvas;
    [SerializeField] Image bossHealthSlider;
    [SerializeField] float bossAgroDistance;
    [SerializeField] float shootRange;
    [SerializeField] float moveSpeed, combatSpeed;
    public GameObject hitEffect;
    int maxBossHealth;

    Vector3 moveDirection;
    float actionCounter;
    float shootCounter;
    int currentAction;
    bool inCombat;

    [Header ("Pathfinding")]
    [SerializeField] float nextWaypointDistance = 3f;
    Path currentPath;
    int currentPathWaypoint;
    Transform pathTarget;
    Seeker seeker;

    private void Awake () {
        instance = this;
        maxBossHealth = currentBossHealth;
    }

    // Start is called before the first frame update
    void Start () {
        actionCounter = actions[currentAction].actionLength;
        seeker = GetComponent<Seeker> ();
        pathTarget = this.transform;
        rigidbody = GetComponent<Rigidbody2D> ();
        InvokeRepeating ("UpdatePath", 0f, .5f);
    }

    // Update is called once per frame
    void Update () {
        float distanceToPlayer = Vector3.Distance (transform.position, PlayerController.instance.transform.position);

        if (!inCombat && distanceToPlayer > bossAgroDistance) {
            return;
        } else {
            inCombat = true;
        }

        if (actionCounter > 0) {
            actionCounter -= Time.deltaTime;

            //Movement
            moveDirection = Vector3.zero;

            if (actions[currentAction].shouldChasePlayer) {
                pathTarget = PlayerController.instance.transform;
                // moveDirection = PlayerController.instance.transform.position - transform.position;
                // moveDirection.Normalize ();
            }

            // if (actions[currentAction].moveToPoint) {
            //     pathTarget = actions[currentAction].pointToMoveTo;
            //     // moveDirection = actions[currentAction].pointToMoveTo.position - transform.position;
            // }

            //Shooting
            if (actions[currentAction].shouldShoot) {
                shootCounter -= Time.deltaTime;
                if (shootCounter <= 0) {
                    shootCounter = actions[currentAction].timeBetweenShoots;

                    foreach (Transform t in actions[currentAction].shootPoints) {
                        Instantiate (actions[currentAction].itemToShoot, t.position, t.rotation);
                    }
                }
            }

            if (currentPath == null) return;
            if (currentPathWaypoint >= currentPath.vectorPath.Count - 1) return;
            if (Vector3.Distance (transform.position, currentPath.vectorPath[currentPathWaypoint]) <= nextWaypointDistance) currentPathWaypoint++;

            if (inCombat) {
                LookAt (PlayerController.instance.transform);
            } else {
                //Look forward to path
                Vector2 rotateDirection = new Vector2 (currentPath.vectorPath[currentPathWaypoint].x - transform.position.x, currentPath.vectorPath[currentPathWaypoint].y - transform.position.y);
                var angle = Mathf.Atan2 (rotateDirection.y, rotateDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
            }

            if (distanceToPlayer > 3f) {
                moveDirection = currentPath.vectorPath[currentPathWaypoint] - transform.position;
                moveDirection.Normalize ();
            } else {
                moveDirection = Vector3.zero;
            }

            rigidbody.velocity = moveDirection * moveSpeed;

        } else {
            currentAction++;
            if (currentAction >= actions.Length) {
                currentAction = 0;
            }

            actionCounter = actions[currentAction].actionLength;
        }
    }

    private void LateUpdate () {
        bossCanvas.transform.position = new Vector2 (transform.position.x, transform.position.y);
    }

    public void TakeDamage (int damageAmount) {
        currentBossHealth -= damageAmount;

        if (currentBossHealth <= 0) {
            gameObject.SetActive (false);
            bossCanvas.SetActive (false);

            Instantiate (deathEffect, transform.position, transform.rotation);

            PlayerHealthController.instance.MakeInvincible (5f);
            Color playerColor = PlayerController.instance.playerBodySR.color;
            PlayerController.instance.playerBodySR.color = new Color (playerColor.r, playerColor.g, playerColor.b, 1f);

            LevelManager.instance.GetMultipleCoins (10);
            LevelManager.instance.StartCoroutine ("LevelEnd");
        }

        bossHealthSlider.fillAmount = (float) currentBossHealth / maxBossHealth;

        Debug.Log ((maxBossHealth - currentBossHealth) / Time.time);
    }

    private void LookAt (Transform target) {
        Vector2 rotateDirection = new Vector2 (target.position.x - transform.position.x, target.position.y - transform.position.y);
        var angle = Mathf.Atan2 (rotateDirection.y, rotateDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
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

    void OnPathComplete (Path p) {
        if (!p.error) {
            currentPath = p;
            currentPathWaypoint = 0;
        }
    }

    void UpdatePath () {
        seeker.StartPath (transform.position, pathTarget.position, OnPathComplete);
    }

}

[System.Serializable]
public class BossAction {
    [Header ("Action")]
    public float actionLength;
    public bool shouldMove;
    public bool shouldChasePlayer;
    public float moveSpeed;
    public bool moveToPoint;
    public Transform pointToMoveTo;
    public bool shouldShoot;
    public GameObject itemToShoot;
    public float timeBetweenShoots;
    public Transform[] shootPoints;
}