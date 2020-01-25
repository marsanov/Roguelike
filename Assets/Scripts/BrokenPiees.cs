using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPiees : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float lifeTime = 1f;
    [SerializeField] float fadeSpeed = 1f;

    Vector3 moveDirection;
    float deceleration = 5f;
    SpriteRenderer spriteSR;

    // Start is called before the first frame update
    void Start()
    {
        moveDirection.x = Random.Range(-moveSpeed, moveSpeed);
        moveDirection.y = Random.Range(-moveSpeed, moveSpeed);
        spriteSR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDirection * Time.deltaTime;
        moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, deceleration * Time.deltaTime);

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            spriteSR.color = new Color(spriteSR.color.r, spriteSR.color.g, spriteSR.color.b, Mathf.MoveTowards(spriteSR.color.a, 0, fadeSpeed * Time.deltaTime));
            if (spriteSR.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
