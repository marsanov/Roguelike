using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] Transform gunArm;

    Rigidbody2D rigidbody;
    Vector2 moveInput = new Vector2();
    Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        rigidbody.velocity = moveInput * moveSpeed;

        Vector3 mousePosition = Input.mousePosition;
        Vector3 screenPoint = camera.WorldToScreenPoint(transform.localPosition);

        if(mousePosition.x < screenPoint.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            gunArm.localScale = new Vector3(-1, -1, 1);
        }
        else
        {
            transform.localScale = Vector3.one;
            gunArm.localScale = Vector3.one;
        }

        //Поворот оружия
        Vector2 offset = new Vector2(mousePosition.x - screenPoint.x, mousePosition.y - screenPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
