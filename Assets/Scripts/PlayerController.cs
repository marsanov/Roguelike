using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    Vector2 input = new Vector2();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        input.y = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        transform.position += new Vector3(input.x, input.y, 0);
    }
}
