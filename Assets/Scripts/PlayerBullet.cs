using UnityEngine;

public class PlayerBullet : MonoBehaviour {
    [SerializeField] float speed = 7.5f;
    [SerializeField] int bulletDamage = 10;
    [SerializeField] GameObject impactEffect;

    Rigidbody2D rigidbody;
    
    private void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        rigidbody.velocity = transform.right * speed;
    }

    
    private void OnTriggerEnter2D(Collider2D other) {
        Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);

        other.GetComponent<EnemyController>().DamaGeEnemy(bulletDamage);
    }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}