using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health = 150;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float chaseDistance = 7f;
    [SerializeField] GameObject[] deathSplatters;
    [SerializeField] GameObject hitEffect;
    [SerializeField] bool shouldShoot;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate;
    [SerializeField] float shootRange = 7f;
    [SerializeField] SpriteRenderer bodySprite;

    Rigidbody2D rigidbody;
    Vector3 moveDirection;
    Animator animator;
    float fireCounter;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
            fireCounter -= Time.deltaTime;
            if (fireCounter <= 0)
            {
                fireCounter = fireRate;
                Instantiate(bullet, firePoint.position, firePoint.rotation);
            }
        }
    }

    private void Move()
    {
        if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < chaseDistance)
        {
            moveDirection = PlayerController.instance.transform.position - transform.position;
        }
        else
        {
            moveDirection = Vector3.zero;
        }

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
        Instantiate(hitEffect, transform.position, transform.rotation);
        if (health <= 0)
        {
            Destroy(gameObject);
            int selectedSplatter = Random.Range(0, deathSplatters.Length - 1);
            int rotation = Random.Range(0, 4);
            Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0, 0, rotation * 90f));
        }
    }
}
