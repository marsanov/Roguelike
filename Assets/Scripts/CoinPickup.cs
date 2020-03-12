using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] int coinValue = 1;
    [SerializeField] float waitToBeCollected = 0.5f;
    [SerializeField] int coinPickupSFX = 5;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && waitToBeCollected <= 0)
        {
            LevelManager.instance.GetCoins(coinValue);
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(coinPickupSFX);
        }
    }
}
