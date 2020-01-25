﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] int healAmount = 50;
    [SerializeField] float waitToBecollected = 0.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && waitToBecollected <= 0)
        {
            PlayerHealthController.instance.HealPlayer(healAmount);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (waitToBecollected > 0)
            waitToBecollected -= Time.deltaTime;
    }
}
