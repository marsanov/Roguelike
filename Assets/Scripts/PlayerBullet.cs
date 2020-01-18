using UnityEngine;

public class PlayerBullet : MonoBehaviour {
    [SerializeField] float speed = 7.5f;
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
    }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}