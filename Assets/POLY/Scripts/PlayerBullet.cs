using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] float moveSpeed = 7.5f;
    [SerializeField] int bulletDamage = 10;
    [SerializeField] GameObject impactEffect;
    [SerializeField] int bulletSoundIndex;
    [SerializeField] int bulletImpactSoundIndex;

    Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        AudioManager.instance.PlaySFX(bulletSoundIndex);
    }

    private void Update()
    {
        rigidbody.velocity = transform.right * moveSpeed;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().DamaGeEnemy(bulletDamage);
        }

        if(other.tag == "Boss")
        {
            BossController.instance.TakeDamage(bulletDamage);
            Instantiate(BossController.instance.hitEffect, transform.position, transform.rotation);
        }

        Instantiate(impactEffect, transform.position, transform.rotation);
        AudioManager.instance.PlaySFX(bulletImpactSoundIndex);
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}