using UnityEngine;

public class EnemyBullet : MonoBehaviour {
    [SerializeField] float speed = 7.5f;
    [SerializeField] int bulletDamage = 10;
    [SerializeField] GameObject impactEffect;

    Vector3 direction;

    private void Start () {
        // direction = PlayerController.instance.transform.position - transform.position;
        // direction.Normalize();

        direction = transform.right;
    }

    private void Update () {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D (Collider2D other) {
        Instantiate (impactEffect, transform.position, transform.rotation);

        if (other.tag == "Player") {
            PlayerHealthController.instance.DamagePlayer (bulletDamage);
        }
        Debug.Log (other);
        Destroy (gameObject);
    }

    private void OnBecameInvisible () {
        Destroy (gameObject);
    }
}