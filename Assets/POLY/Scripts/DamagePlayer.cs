using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] int damage = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        TriggerDealDamage(other);
    }

    private void TriggerDealDamage(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlayer(damage);
        }
    }
    private void CollisionDealDamage(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlayer(damage);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TriggerDealDamage(other);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        CollisionDealDamage(other);
    }

    private void OnCollisionStay2D(Collision2D other) {
        CollisionDealDamage(other);
    }
}
