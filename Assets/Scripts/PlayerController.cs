using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 15f;

    Rigidbody2D rigidbody;
    Vector2 moveInput = new Vector2();

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        moveInput.y = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        rigidbody.velocity = moveInput * moveSpeed;
    }
}
