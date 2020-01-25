using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] int breackSound;
    [SerializeField] GameObject[] brokenPieces;
    [SerializeField] int maxPieces = 3;
    [SerializeField] bool shouldDropItems;
    [SerializeField] GameObject[] itemsToDrop;
    [SerializeField] float dropPercentChance;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (PlayerController.instance.dashCounter > 0)
            {
                Smash();
            }
        }
        if(other.tag == "PlayerBullet")
        {
            Smash();
        }
    }

    private void Smash()
    {
        //Дроп кусочков боксов
        int piecesToDrop = Random.Range(1, maxPieces);
        for (int i = 0; i < piecesToDrop; i++)
        {
            int randomPiece = Random.Range(0, brokenPieces.Length);
            Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);
        }

        //Дроп лута
        if (shouldDropItems)
        {
            float dropChance = Random.Range(0f, 100f);
            if (dropChance < dropPercentChance)
            {
                int randomItem = Random.Range(0, itemsToDrop.Length);
                Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
            }
        }
        Destroy(gameObject);
        AudioManager.instance.PlaySFX(breackSound);
    }
}
